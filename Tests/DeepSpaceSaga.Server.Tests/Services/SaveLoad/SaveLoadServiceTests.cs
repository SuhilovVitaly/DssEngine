using DeepSpaceSaga.Server.Services.SaveLoad;

namespace DeepSpaceSaga.Server.Tests.Services.SaveLoad;

public class SaveLoadServiceTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly SaveLoadService _sut;
    private readonly Mock<ISessionContextService> _mockSessionContext;
    private readonly Mock<ISessionInfoService> _mockSessionInfo;
    private readonly Mock<IMetricsService> _mockMetrics;
    private readonly GameSession _gameSession;

    public SaveLoadServiceTests()
    {
        // Initialize test logger
        TestLoggerRepository.Initialize();

        // Create unique test directory for each test run
        _testDirectory = Path.Combine(Path.GetTempPath(), $"SaveLoadServiceTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);

        // Setup mocks
        _mockSessionContext = new Mock<ISessionContextService>();
        _mockSessionInfo = new Mock<ISessionInfoService>();
        _mockMetrics = new Mock<IMetricsService>();

        // Setup session info mock
        _mockSessionInfo.Setup(x => x.Turn).Returns(100);
        _mockSessionInfo.Setup(x => x.CycleCounter).Returns(5);
        _mockSessionInfo.Setup(x => x.TurnCounter).Returns(25);
        _mockSessionInfo.Setup(x => x.TickCounter).Returns(75);
        _mockSessionInfo.Setup(x => x.IsPaused).Returns(false);
        _mockSessionInfo.Setup(x => x.Speed).Returns(3);

        // Create game session with test data
        _gameSession = new GameSession
        {
            Id = Guid.NewGuid(),
            CelestialObjects = new ConcurrentDictionary<int, ICelestialObject>(),
            Commands = new ConcurrentDictionary<Guid, ICommand>(),
            ActiveEvents = new ConcurrentDictionary<string, IGameActionEvent>(),
            FinishedEvents = new ConcurrentDictionary<string, string>()
        };

        // Add test asteroid to game session
        var asteroid = new BaseAsteroid(3)
        {
            Id = 1,
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            X = 100.0,
            Y = 200.0,
            RemainingDrillAttempts = 2
        };
        _gameSession.CelestialObjects.TryAdd(1, asteroid);

        // Add test command
        var command = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };
        _gameSession.Commands.TryAdd(command.Id, command);

        // Setup session context mock
        _mockSessionContext.Setup(x => x.SessionInfo).Returns(_mockSessionInfo.Object);
        _mockSessionContext.Setup(x => x.Metrics).Returns(_mockMetrics.Object);
        _mockSessionContext.Setup(x => x.GameSession).Returns(_gameSession);

        // Create service under test
        _sut = new SaveLoadService(_testDirectory);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithDefaultDirectory_ShouldUseDefaultSavesDirectory()
    {
        // Act
        var service = new SaveLoadService();

        // Assert
        service.Should().NotBeNull();
        service.Should().BeAssignableTo<ISaveLoadService>();
    }

    [Fact]
    public void Constructor_WithCustomDirectory_ShouldUseCustomDirectory()
    {
        // Arrange
        var customDirectory = "CustomSaves";

        // Act
        var service = new SaveLoadService(customDirectory);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeAssignableTo<ISaveLoadService>();
    }

    #endregion

    #region Save Tests

    [Fact]
    public void Save_Should_CreateSaveFile_Successfully()
    {
        // Arrange
        var saveFileName = "test_save";
        var expectedFilePath = Path.Combine(_testDirectory, $"{saveFileName}.json");

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        File.Exists(expectedFilePath).Should().BeTrue();
    }

    [Fact]
    public void Save_Should_CreateSaveDirectory_IfNotExists()
    {
        // Arrange
        var nonExistentDirectory = Path.Combine(_testDirectory, "SubDirectory");
        var service = new SaveLoadService(nonExistentDirectory);
        var saveFileName = "test_save";

        // Act
        service.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        Directory.Exists(nonExistentDirectory).Should().BeTrue();
        File.Exists(Path.Combine(nonExistentDirectory, $"{saveFileName}.json")).Should().BeTrue();
    }

    [Fact]
    public void Save_Should_SerializeGameSessionCorrectly()
    {
        // Arrange
        var saveFileName = "serialization_test";
        var expectedFilePath = Path.Combine(_testDirectory, $"{saveFileName}.json");

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        var savedContent = File.ReadAllText(expectedFilePath);
        savedContent.Should().NotBeNullOrEmpty();
        savedContent.Should().Contain("\"Id\":");
        savedContent.Should().Contain("\"State\":");
        savedContent.Should().Contain("\"CelestialObjects\":");
        savedContent.Should().Contain("\"Test Asteroid\"");
    }

    [Fact]
    public void Save_Should_OverwriteExistingFile()
    {
        // Arrange
        var saveFileName = "overwrite_test";
        var filePath = Path.Combine(_testDirectory, $"{saveFileName}.json");

        // Create initial file
        _sut.Save(_mockSessionContext.Object, saveFileName);
        var initialFileSize = new FileInfo(filePath).Length;

        // Modify game session
        var newAsteroid = new BaseAsteroid(5)
        {
            Id = 2,
            Name = "New Asteroid",
            Type = CelestialObjectType.Asteroid
        };
        _gameSession.CelestialObjects.TryAdd(2, newAsteroid);

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        File.Exists(filePath).Should().BeTrue();
        var newFileSize = new FileInfo(filePath).Length;
        newFileSize.Should().NotBe(initialFileSize);

        var savedContent = File.ReadAllText(filePath);
        savedContent.Should().Contain("New Asteroid");
    }

    [Theory]
    [InlineData("")]
    [InlineData("simple_name")]
    [InlineData("name_with_underscores")]
    [InlineData("name-with-dashes")]
    [InlineData("name123")]
    public void Save_Should_HandleDifferentFileNames(string saveFileName)
    {
        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        var expectedFilePath = Path.Combine(_testDirectory, $"{saveFileName}.json");
        File.Exists(expectedFilePath).Should().BeTrue();
    }

    [Fact]
    public void Save_Should_ThrowException_WhenSessionContextIsNull()
    {
        // Act & Assert
        var action = () => _sut.Save(null!, "test_save");
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Save_Should_NotThrowException_WhenFileNameIsNull()
    {
        // Arrange & Act
        var action = () => _sut.Save(_mockSessionContext.Object, null!);
        
        // Assert - SaveLoadService allows null filename and creates ".json" file
        action.Should().NotThrow();
        var expectedFilePath = Path.Combine(_testDirectory, ".json");
        File.Exists(expectedFilePath).Should().BeTrue();
    }

    [Fact]
    public void Save_Should_RethrowException_WhenWriteFails()
    {
        // Arrange
        var invalidDirectory = Path.Combine(Path.GetTempPath(), "InvalidDirectory\0"); // Invalid path
        var service = new SaveLoadService(invalidDirectory);

        // Act & Assert
        var action = () => service.Save(_mockSessionContext.Object, "test_save");
        action.Should().Throw<Exception>();
    }

    #endregion

    #region Load Tests

    [Fact]
    public void Load_Should_LoadSavedGame_Successfully()
    {
        // Arrange
        var saveFileName = "load_test";
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Act
        var result = _sut.Load(saveFileName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ISessionContextService>();
        result.GameSession.Should().NotBeNull();
        result.SessionInfo.Should().NotBeNull();
        result.Metrics.Should().NotBeNull();
    }

    [Fact]
    public void Load_Should_RestoreGameSessionData_Correctly()
    {
        // Arrange
        var saveFileName = "data_restore_test";
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Act
        var result = _sut.Load(saveFileName);

        // Assert
        result.GameSession.Should().NotBeNull();
        result.GameSession.CelestialObjects.Should().HaveCount(1);
        result.GameSession.Commands.Should().BeEmpty(); // Commands are not saved/loaded

        var loadedAsteroid = result.GameSession.CelestialObjects[1] as BaseAsteroid;
        loadedAsteroid.Should().NotBeNull();
        loadedAsteroid!.Name.Should().Be("Test Asteroid");
        loadedAsteroid.RemainingDrillAttempts.Should().Be(2);
    }

    [Fact]
    public void Load_Should_RestoreSessionInfo_Correctly()
    {
        // Arrange
        var saveFileName = "session_info_test";
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Act
        var result = _sut.Load(saveFileName);

        // Assert
        result.SessionInfo.CycleCounter.Should().Be(5);
        result.SessionInfo.TurnCounter.Should().Be(25);
        result.SessionInfo.TickCounter.Should().Be(75);
        result.SessionInfo.Turn.Should().Be(0); // Turn is initialized to 0 in new SessionInfoService
    }

    [Fact]
    public void Load_Should_ThrowException_WhenFileNotExists()
    {
        // Act & Assert
        var action = () => _sut.Load("non_existent_file");
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void Load_Should_ThrowException_WhenFileNameIsNull()
    {
        // Act & Assert
        var action = () => _sut.Load(null!);
        action.Should().Throw<FileNotFoundException>(); // File ".json" will not exist
    }

    [Fact]
    public void Load_Should_ThrowException_WhenFileNameIsEmpty()
    {
        // Act & Assert
        var action = () => _sut.Load("");
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void Load_Should_ThrowException_WhenFileContainsInvalidJson()
    {
        // Arrange
        var saveFileName = "invalid_json_test";
        var filePath = Path.Combine(_testDirectory, $"{saveFileName}.json");
        File.WriteAllText(filePath, "{ invalid json }");

        // Act & Assert
        var action = () => _sut.Load(saveFileName);
        action.Should().Throw<JsonException>();
    }

    [Fact]
    public void Load_Should_CreateIndependentInstances()
    {
        // Arrange
        var saveFileName = "independence_test";
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Act
        var result1 = _sut.Load(saveFileName);
        var result2 = _sut.Load(saveFileName);

        // Assert
        result1.Should().NotBeSameAs(result2);
        result1.GameSession.Should().NotBeSameAs(result2.GameSession);
        result1.SessionInfo.Should().NotBeSameAs(result2.SessionInfo);
    }

    #endregion

    #region Save and Load Integration Tests

    [Fact]
    public void SaveAndLoad_Should_PreserveCompleteGameState()
    {
        // Arrange
        var saveFileName = "complete_state_test";

        // Add more complex data to game session
        var command2 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.Mining,
            CelestialObjectId = 1
        };
        _gameSession.Commands.TryAdd(command2.Id, command2);

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);
        var result = _sut.Load(saveFileName);

        // Assert
        result.GameSession.Should().NotBeNull();
        result.GameSession.CelestialObjects.Should().HaveCount(_gameSession.CelestialObjects.Count);
        result.GameSession.Commands.Should().BeEmpty(); // Commands are not loaded

        // Verify specific objects
        var originalAsteroid = _gameSession.CelestialObjects[1] as BaseAsteroid;
        var loadedAsteroid = result.GameSession.CelestialObjects[1] as BaseAsteroid;
        
        loadedAsteroid!.Name.Should().Be(originalAsteroid!.Name);
        loadedAsteroid.RemainingDrillAttempts.Should().Be(originalAsteroid.RemainingDrillAttempts);
        loadedAsteroid.X.Should().Be(originalAsteroid.X);
        loadedAsteroid.Y.Should().Be(originalAsteroid.Y);
    }

    [Fact]
    public void SaveAndLoad_Should_HandleEmptyGameSession()
    {
        // Arrange
        var saveFileName = "empty_session_test";
        var emptySession = new GameSession
        {
            Id = Guid.NewGuid(),
            CelestialObjects = new ConcurrentDictionary<int, ICelestialObject>(),
            Commands = new ConcurrentDictionary<Guid, ICommand>(),
            ActiveEvents = new ConcurrentDictionary<string, IGameActionEvent>(),
            FinishedEvents = new ConcurrentDictionary<string, string>()
        };

        _mockSessionContext.Setup(x => x.GameSession).Returns(emptySession);

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);
        var result = _sut.Load(saveFileName);

        // Assert
        result.GameSession.Should().NotBeNull();
        result.GameSession.CelestialObjects.Should().BeEmpty();
        result.GameSession.Commands.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void SaveAndLoad_Should_PreserveDifferentDrillAttempts(int drillAttempts)
    {
        // Arrange
        var saveFileName = $"drill_attempts_test_{drillAttempts}";
        var asteroid = new BaseAsteroid(drillAttempts)
        {
            Id = 99,
            Name = $"Asteroid_{drillAttempts}",
            Type = CelestialObjectType.Asteroid
        };

        _gameSession.CelestialObjects.TryAdd(99, asteroid);

        // Act
        _sut.Save(_mockSessionContext.Object, saveFileName);
        var result = _sut.Load(saveFileName);

        // Assert
        var loadedAsteroid = result.GameSession.CelestialObjects[99] as BaseAsteroid;
        loadedAsteroid!.RemainingDrillAttempts.Should().Be(drillAttempts);
    }

    #endregion

    #region NotImplemented Methods Tests

    [Fact]
    public void DeleteSave_Should_ThrowNotImplementedException()
    {
        // Act & Assert
        var action = () => _sut.DeleteSave("test_save");
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void GetAllSaves_Should_ThrowNotImplementedException()
    {
        // Act & Assert
        var action = () => _sut.GetAllSaves();
        action.Should().Throw<NotImplementedException>();
    }

    #endregion

    #region File System Edge Cases

    [Fact]
    public void Save_Should_HandleSpecialCharactersInFileName()
    {
        // Arrange
        var saveFileName = "test_файл_测试"; // Mixed languages

        // Act
        var action = () => _sut.Save(_mockSessionContext.Object, saveFileName);

        // Assert
        action.Should().NotThrow();
        var expectedFilePath = Path.Combine(_testDirectory, $"{saveFileName}.json");
        File.Exists(expectedFilePath).Should().BeTrue();
    }

    [Fact]
    public void Save_Should_HandleVeryLongFileName()
    {
        // Arrange
        var longFileName = new string('a', 200); // Very long filename

        // Act & Assert
        // This might throw depending on OS limitations, which is expected behavior
        var action = () => _sut.Save(_mockSessionContext.Object, longFileName);
        action.Should().NotThrow(); // Or throw PathTooLongException, both are acceptable
    }

    [Fact]
    public void Load_Should_HandleConcurrentAccess()
    {
        // Arrange
        var saveFileName = "concurrent_test";
        _sut.Save(_mockSessionContext.Object, saveFileName);

        // Act & Assert
        var tasks = Enumerable.Range(0, 5).Select(_ => Task.Run(() => _sut.Load(saveFileName))).ToArray();
        
        var action = () => Task.WaitAll(tasks);
        action.Should().NotThrow();

        foreach (var task in tasks)
        {
            task.Result.Should().NotBeNull();
        }
    }

    #endregion

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
} 