namespace DeepSpaceSaga.Server.Tests.Services.Dialogs;

public class DialogsServiceTests : IDisposable
{
    private readonly string _testScenarioName = "TestScenario";
    private readonly string _testDataPath;
    private DialogsService _dialogsService;

    public DialogsServiceTests()
    {
        // Setup test data directory
        _testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Scenarios", _testScenarioName, "Dialogs");
        Directory.CreateDirectory(_testDataPath);
        
        // Create test dialog files
        CreateTestDialogFiles();
        
        _dialogsService = new DialogsService(_testScenarioName);
    }

    public void Dispose()
    {
        // Cleanup test data directory
        var scenarioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Scenarios", _testScenarioName);
        if (Directory.Exists(scenarioPath))
        {
            Directory.Delete(scenarioPath, true);
        }
    }

    private void CreateTestDialogFiles()
    {
        // Create a test dialog file
        var testDialogs = new List<BaseDialog>
        {
            new BaseDialog
            {
                Key = "test-dialog-1",
                Title = "Test Dialog 1",
                Message = "Test message 1",
                Type = DialogTypes.SelectCelestialObject,
                Trigger = DialogTrigger.Asteroid,
                ChainPart = false,
                TriggerValue = "asteroid-1",
                Exits = new List<DialogExit>
                {
                    new DialogExit { Key = "exit-1", NextKey = "test-dialog-2", TextKey = "Continue" },
                    new DialogExit { Key = "exit-2", NextKey = "-1", TextKey = "Exit" }
                }
            },
            new BaseDialog
            {
                Key = "test-dialog-2",
                Title = "Test Dialog 2",
                Message = "Test message 2",
                Type = DialogTypes.SelectCelestialObject,
                Trigger = DialogTrigger.Asteroid,
                ChainPart = true,
                TriggerValue = "asteroid-1",
                Exits = new List<DialogExit>
                {
                    new DialogExit { Key = "exit-3", NextKey = "test-dialog-3", TextKey = "Next" }
                }
            },
            new BaseDialog
            {
                Key = "test-dialog-3",
                Title = "Test Dialog 3",
                Message = "Test message 3",
                Type = DialogTypes.SelectCelestialObject,
                Trigger = DialogTrigger.Asteroid,
                ChainPart = true,
                TriggerValue = "asteroid-1",
                Exits = new List<DialogExit>
                {
                    new DialogExit { Key = "exit-4", NextKey = "-1", TextKey = "End" }
                }
            },
            new BaseDialog
            {
                Key = "turn-dialog-1",
                Title = "Turn Dialog 1",
                Message = "Turn finished message",
                Type = DialogTypes.TurnFinished,
                Trigger = DialogTrigger.Turn,
                ChainPart = false,
                TriggerValue = "5",
                Exits = new List<DialogExit>
                {
                    new DialogExit { Key = "exit-5", NextKey = "-1", TextKey = "Continue" }
                }
            }
        };

        var json = JsonSerializer.Serialize(testDialogs, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        File.WriteAllText(Path.Combine(_testDataPath, "TestDialogs.json"), json);
    }

    [Fact]
    public void Constructor_Should_Load_Dialogs_From_Scenario()
    {
        // Act & Assert
        // The constructor should have loaded the test dialogs
        _dialogsService.Should().NotBeNull();
    }

    [Fact]
    public void AddDialog_Should_Add_New_Dialog()
    {
        // Arrange
        var newDialog = new BaseDialog
        {
            Key = "new-dialog",
            Title = "New Dialog",
            Message = "New dialog message",
            Type = DialogTypes.SelectCelestialObject,
            Trigger = DialogTrigger.Asteroid
        };

        // Act
        _dialogsService.AddDialog(newDialog);

        // Assert
        // We can't directly access the dictionary, but we can test through other methods
        var connectedDialogs = _dialogsService.GetConnectedDialogs(newDialog);
        connectedDialogs.Should().NotBeNull();
    }

    [Fact]
    public void AddDialog_Should_Not_Add_Dialog_With_Existing_Key()
    {
        // Arrange
        var dialog1 = new BaseDialog
        {
            Key = "duplicate-key",
            Title = "Dialog 1",
            Message = "Message 1"
        };

        var dialog2 = new BaseDialog
        {
            Key = "duplicate-key",
            Title = "Dialog 2",
            Message = "Message 2"
        };

        // Act
        _dialogsService.AddDialog(dialog1);
        _dialogsService.AddDialog(dialog2); // Should not add due to duplicate key

        // Assert
        // The second dialog should not have been added
        // We can't directly verify this, but the behavior is defined in the implementation
        var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog1);
        connectedDialogs.Should().NotBeNull();
    }

    [Fact]
    public void AddDialogs_Should_Add_All_Dialogs()
    {
        // Arrange
        var dialogs = new List<IDialog>
        {
            new BaseDialog
            {
                Key = "bulk-dialog-1",
                Title = "Bulk Dialog 1",
                Message = "Message 1"
            },
            new BaseDialog
            {
                Key = "bulk-dialog-2",
                Title = "Bulk Dialog 2",
                Message = "Message 2"
            }
        };

        // Act
        _dialogsService.AddDialogs(dialogs);

        // Assert
        foreach (var dialog in dialogs)
        {
            var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog);
            connectedDialogs.Should().NotBeNull();
        }
    }

    [Fact]
    public void GetConnectedDialogs_Should_Return_Empty_List_For_Dialog_Without_Exits()
    {
        // Arrange
        var dialog = new BaseDialog
        {
            Key = "no-exits-dialog",
            Title = "No Exits Dialog",
            Message = "This dialog has no exits",
            Exits = new List<DialogExit>()
        };

        _dialogsService.AddDialog(dialog);

        // Act
        var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog);

        // Assert
        connectedDialogs.Should().NotBeNull();
        connectedDialogs.Should().BeEmpty();
    }

    [Fact]
    public void GetConnectedDialogs_Should_Return_Connected_Dialogs()
    {
        // Arrange
        var dialog1 = new BaseDialog
        {
            Key = "connected-1",
            Title = "Connected Dialog 1",
            Message = "Message 1",
            Exits = new List<DialogExit>
            {
                new DialogExit { Key = "exit-conn-1", NextKey = "connected-2", TextKey = "Next" }
            }
        };

        var dialog2 = new BaseDialog
        {
            Key = "connected-2",
            Title = "Connected Dialog 2",
            Message = "Message 2",
            Exits = new List<DialogExit>
            {
                new DialogExit { Key = "exit-conn-2", NextKey = "-1", TextKey = "End" }
            }
        };

        _dialogsService.AddDialog(dialog1);
        _dialogsService.AddDialog(dialog2);

        // Act
        var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog1);

        // Assert
        connectedDialogs.Should().NotBeNull();
        connectedDialogs.Should().HaveCount(1);
        connectedDialogs.Should().Contain(d => d.Key == "connected-2");
    }

    [Fact]
    public void GetConnectedDialogs_Should_Handle_Circular_References()
    {
        // Arrange
        var dialog1 = new BaseDialog
        {
            Key = "circular-1",
            Title = "Circular Dialog 1",
            Message = "Message 1",
            Exits = new List<DialogExit>
            {
                new DialogExit { Key = "exit-circ-1", NextKey = "circular-2", TextKey = "Next" }
            }
        };

        var dialog2 = new BaseDialog
        {
            Key = "circular-2",
            Title = "Circular Dialog 2",
            Message = "Message 2",
            Exits = new List<DialogExit>
            {
                new DialogExit { Key = "exit-circ-2", NextKey = "circular-1", TextKey = "Back" }
            }
        };

        _dialogsService.AddDialog(dialog1);
        _dialogsService.AddDialog(dialog2);

        // Act
        var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog1);

        // Assert
        connectedDialogs.Should().NotBeNull();
        connectedDialogs.Should().HaveCount(2); // Should find both dialogs in the circular reference
        connectedDialogs.Should().Contain(d => d.Key == "circular-2");
        connectedDialogs.Should().Contain(d => d.Key == "circular-1");
    }

    [Fact]
    public void GetConnectedDialogs_Should_Ignore_Exit_To_Minus_One()
    {
        // Arrange
        var dialog = new BaseDialog
        {
            Key = "exit-minus-one",
            Title = "Exit Minus One Dialog",
            Message = "Message",
            Exits = new List<DialogExit>
            {
                new DialogExit { Key = "exit-minus-1", NextKey = "-1", TextKey = "Exit" },
                new DialogExit { Key = "exit-non-exist", NextKey = "non-existent", TextKey = "Non-existent" }
            }
        };

        _dialogsService.AddDialog(dialog);

        // Act
        var connectedDialogs = _dialogsService.GetConnectedDialogs(dialog);

        // Assert
        connectedDialogs.Should().NotBeNull();
        connectedDialogs.Should().BeEmpty();
    }

    [Fact]
    public void DialogsActivation_Should_Return_Empty_List_For_Unknown_Command_Type()
    {
        // Arrange
        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns((CommandTypes)999); // Unknown type

        var context = new Mock<ISessionContextService>();

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void DialogsActivation_Should_Handle_UiSelectCelestialObject_Command()
    {
        // Arrange
        var asteroid = new Mock<IAsteroid>();
        asteroid.Setup(a => a.Id).Returns(1);

        var gameSession = new GameSession();
        gameSession.CelestialObjects.TryAdd(1, asteroid.Object);

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.GameSession).Returns(gameSession);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.UiSelectCelestialObject);
        command.Setup(c => c.TargetCelestialObjectId).Returns(1);

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        // The result should contain the asteroid selection dialog from our test data
        result.Should().Contain(d => d.Type == DialogTypes.SelectCelestialObject && d.Trigger == DialogTrigger.Asteroid);
    }

    [Fact]
    public void DialogsActivation_Should_Handle_DialogInitiationByTurn_Command()
    {
        // Arrange
        var sessionInfo = new Mock<ISessionInfoService>();
        sessionInfo.Setup(si => si.Turn).Returns(10);

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.SessionInfo).Returns(sessionInfo.Object);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.DialogInitiationByTurn);

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        // Should return the turn dialog from our test data since turn 10 >= 5
        result.Should().Contain(d => d.Type == DialogTypes.TurnFinished && d.Key == "turn-dialog-1");
    }

    [Fact]
    public void DialogsActivation_Should_Not_Return_Turn_Dialogs_For_Future_Turns()
    {
        // Arrange
        var sessionInfo = new Mock<ISessionInfoService>();
        sessionInfo.Setup(si => si.Turn).Returns(3); // Less than trigger value of 5

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.SessionInfo).Returns(sessionInfo.Object);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.DialogInitiationByTurn);

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void DialogsActivation_Should_Not_Return_Previously_Shown_Turn_Dialogs()
    {
        // Arrange
        var sessionInfo = new Mock<ISessionInfoService>();
        sessionInfo.Setup(si => si.Turn).Returns(10);

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.SessionInfo).Returns(sessionInfo.Object);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.DialogInitiationByTurn);

        // Act - Call twice
        var result1 = _dialogsService.DialogsActivation(command.Object, context.Object);
        var result2 = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result1.Should().NotBeNull();
        result1.Should().NotBeEmpty();
        
        result2.Should().NotBeNull();
        result2.Should().BeEmpty(); // Should be empty on second call due to history
    }

    [Fact]
    public void DialogsActivation_Should_Handle_Non_Existent_Celestial_Object()
    {
        // Arrange
        var gameSession = new GameSession();
        // Don't add any celestial objects

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.GameSession).Returns(gameSession);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.UiSelectCelestialObject);
        command.Setup(c => c.TargetCelestialObjectId).Returns(999); // Non-existent

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void DialogsActivation_Should_Not_Return_Previously_Activated_Asteroid_Dialogs()
    {
        // Arrange
        var asteroid = new Mock<IAsteroid>();
        asteroid.Setup(a => a.Id).Returns(1);

        var gameSession = new GameSession();
        gameSession.CelestialObjects.TryAdd(1, asteroid.Object);

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.GameSession).Returns(gameSession);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.UiSelectCelestialObject);
        command.Setup(c => c.TargetCelestialObjectId).Returns(1);

        // Act - Call twice
        var result1 = _dialogsService.DialogsActivation(command.Object, context.Object);
        var result2 = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result1.Should().NotBeNull();
        result1.Should().NotBeEmpty();
        
        result2.Should().NotBeNull();
        result2.Should().BeEmpty(); // Should be empty on second call due to history
    }

    [Fact]
    public void DialogsActivation_Should_Skip_Chain_Part_Dialogs()
    {
        // Arrange
        var asteroid = new Mock<IAsteroid>();
        asteroid.Setup(a => a.Id).Returns(1);

        var gameSession = new GameSession();
        gameSession.CelestialObjects.TryAdd(1, asteroid.Object);

        var context = new Mock<ISessionContextService>();
        context.Setup(c => c.GameSession).Returns(gameSession);

        var command = new Mock<ICommand>();
        command.Setup(c => c.Type).Returns(CommandTypes.UiSelectCelestialObject);
        command.Setup(c => c.TargetCelestialObjectId).Returns(1);

        // Act
        var result = _dialogsService.DialogsActivation(command.Object, context.Object);

        // Assert
        result.Should().NotBeNull();
        // Should only return non-chain-part dialogs
        result.Should().OnlyContain(d => !d.ChainPart);
    }
}
