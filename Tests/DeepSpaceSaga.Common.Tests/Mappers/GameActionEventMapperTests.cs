namespace DeepSpaceSaga.Common.Tests.Mappers;

public class GameActionEventMapperTests
{
    [Fact]
    public void ToDto_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            CalculationTurnId = 456L,
            CelestialObjectId = 789L,
            ModuleId = 101L,
            TargetObjectId = 112L,
            EventType = GameActionEventTypes.HelpSystem,
            Dialog = CreateTestDialog("main_dialog"),
            ConnectedDialogs = new List<IDialog>
            {
                CreateTestDialog("dialog1"),
                CreateTestDialog("dialog2")
            }
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.Key.Should().Be("123");
        result.CalculationTurnId.Should().Be(456L);
        result.CelestialObjectId.Should().Be(789L);
        result.ModuleId.Should().Be(101L);
        result.TargetObjectId.Should().Be(112L);
        result.EventType.Should().Be(GameActionEventTypes.HelpSystem);
        result.Dialog.Should().NotBeNull();
        result.Dialog!.Key.Should().Be("main_dialog");
        result.ConnectedDialogs.Should().HaveCount(2);
        result.ConnectedDialogs[0].Key.Should().Be("dialog1");
        result.ConnectedDialogs[1].Key.Should().Be("dialog2");
    }

    [Fact]
    public void ToDto_Should_Handle_Empty_ConnectedDialogs()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            CalculationTurnId = 456L,
            EventType = GameActionEventTypes.None,
            Dialog = CreateTestDialog("main"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.ConnectedDialogs.Should().BeEmpty();
        result.Dialog.Should().NotBeNull();
    }

    [Fact]
    public void ToDto_Should_Handle_Null_Optional_Properties()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            CalculationTurnId = 456L,
            CelestialObjectId = null,
            ModuleId = null,
            TargetObjectId = null,
            EventType = GameActionEventTypes.None,
            Dialog = CreateTestDialog("test_dialog"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjectId.Should().BeNull();
        result.ModuleId.Should().BeNull();
        result.TargetObjectId.Should().BeNull();
    }

    [Fact]
    public void ToDto_Should_Map_Different_EventTypes()
    {
        // Arrange
        var gameEventNone = new BaseGameEvent
        {
            Key = "1",
            EventType = GameActionEventTypes.None,
            Dialog = CreateTestDialog("dialog_none"),
            ConnectedDialogs = new List<IDialog>()
        };

        var gameEventHelp = new BaseGameEvent
        {
            Key = "2",
            EventType = GameActionEventTypes.HelpSystem,
            Dialog = CreateTestDialog("dialog_help"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var resultNone = GameActionEventMapper.ToDto(gameEventNone);
        var resultHelp = GameActionEventMapper.ToDto(gameEventHelp);

        // Assert
        resultNone.EventType.Should().Be(GameActionEventTypes.None);
        resultHelp.EventType.Should().Be(GameActionEventTypes.HelpSystem);
    }

    [Fact]
    public void ToDto_Should_Create_Independent_ConnectedDialogs_Copy()
    {
        // Arrange
        var dialogList = new List<IDialog>
        {
            CreateTestDialog("dialog1"),
            CreateTestDialog("dialog2")
        };

        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            Dialog = CreateTestDialog("main_dialog"),
            ConnectedDialogs = dialogList
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Modify original list
        dialogList.Clear();

        // Assert
        result.ConnectedDialogs.Should().HaveCount(2);
        result.ConnectedDialogs[0].Key.Should().Be("dialog1");
        result.ConnectedDialogs[1].Key.Should().Be("dialog2");
    }

    [Fact]
    public void ToDto_Should_Not_Return_Null()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Dialog = CreateTestDialog("test_dialog"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.ConnectedDialogs.Should().NotBeNull();
    }

    [Fact]
    public void ToDto_Should_Handle_Large_Values()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Key = "max",
            CalculationTurnId = long.MaxValue - 1,
            CelestialObjectId = long.MaxValue - 2,
            ModuleId = long.MaxValue - 3,
            TargetObjectId = long.MaxValue - 4,
            EventType = GameActionEventTypes.HelpSystem,
            Dialog = CreateTestDialog("large_values_dialog"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.Key.Should().Be("max");
        result.CalculationTurnId.Should().Be(long.MaxValue - 1);
        result.CelestialObjectId.Should().Be(long.MaxValue - 2);
        result.ModuleId.Should().Be(long.MaxValue - 3);
        result.TargetObjectId.Should().Be(long.MaxValue - 4);
    }

    [Fact]
    public void ToDto_Should_Handle_Zero_And_Negative_Values()
    {
        // Arrange
        var gameEvent = new BaseGameEvent
        {
            Key = "0",
            CalculationTurnId = -1L,
            CelestialObjectId = -100L,
            ModuleId = -200L,
            TargetObjectId = -300L,
            EventType = GameActionEventTypes.None,
            Dialog = CreateTestDialog("negative_values_dialog"),
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Should().NotBeNull();
        result.Key.Should().Be("0");
        result.CalculationTurnId.Should().Be(-1L);
        result.CelestialObjectId.Should().Be(-100L);
        result.ModuleId.Should().Be(-200L);
        result.TargetObjectId.Should().Be(-300L);
    }

    [Fact]
    public void ToDto_Should_Map_Multiple_ConnectedDialogs()
    {
        // Arrange
        var dialogs = new List<IDialog>();
        for (int i = 0; i < 5; i++)
        {
            dialogs.Add(CreateTestDialog($"dialog_{i}"));
        }

        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            Dialog = CreateTestDialog("main_dialog"),
            ConnectedDialogs = dialogs
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.ConnectedDialogs.Should().HaveCount(5);
        for (int i = 0; i < 5; i++)
        {
            result.ConnectedDialogs[i].Key.Should().Be($"dialog_{i}");
        }
    }

    [Fact]
    public void ToDto_Should_Map_Dialog_Properties_Correctly()
    {
        // Arrange
        var testDialog = CreateTestDialog("test_key");
        var gameEvent = new BaseGameEvent
        {
            Key = "123",
            Dialog = testDialog,
            ConnectedDialogs = new List<IDialog>()
        };

        // Act
        var result = GameActionEventMapper.ToDto(gameEvent);

        // Assert
        result.Dialog.Should().NotBeNull();
        result.Dialog!.Key.Should().Be("test_key");
        result.Dialog.Title.Should().Be("Title for test_key");
        result.Dialog.Message.Should().Be("Message for test_key");
        result.Dialog.Type.Should().Be(DialogTypes.SelectCelestialObject);
        result.Dialog.Trigger.Should().Be(DialogTrigger.None);
        result.Dialog.UiScreenType.Should().Be(GameEventUiScreenType.DialogOnePerson);
    }

    private static IDialog CreateTestDialog(string key)
    {
        return new BaseDialog
        {
            Key = key,
            Title = $"Title for {key}",
            Message = $"Message for {key}",
            ChainPart = false,
            Type = DialogTypes.SelectCelestialObject,
            Trigger = DialogTrigger.None,
            UiScreenType = GameEventUiScreenType.DialogOnePerson,
            TriggerValue = "test_value",
            Reporter = new CrewMember
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Character",
                Age = 25,
                Gender = Gender.Male,
                Rank = "Test Rank",
                Portrait = "test_portrait.png",
                Salary = 1000,
                Skills = new Dictionary<CharacterSkillType, ICharacterSkill>()
            },
            Exits = new List<DialogExit>()
        };
    }
} 