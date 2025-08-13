namespace DeepSpaceSaga.Server.Tests.Services.Dialogs;

public class DialogsHistoryTests
{
    [Fact]
    public void Add_Should_Add_Dialog_Key_To_History()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var dialogKey = "test-dialog-key";

        // Act
        dialogsHistory.Add(dialogKey);

        // Assert
        dialogsHistory.IsExist(dialogKey).Should().BeTrue();
    }

    [Fact]
    public void Add_Should_Handle_Multiple_Different_Keys()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var key1 = "dialog-key-1";
        var key2 = "dialog-key-2";
        var key3 = "dialog-key-3";

        // Act
        dialogsHistory.Add(key1);
        dialogsHistory.Add(key2);
        dialogsHistory.Add(key3);

        // Assert
        dialogsHistory.IsExist(key1).Should().BeTrue();
        dialogsHistory.IsExist(key2).Should().BeTrue();
        dialogsHistory.IsExist(key3).Should().BeTrue();
    }

    [Fact]
    public void Add_Should_Handle_Duplicate_Keys()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var dialogKey = "duplicate-key";

        // Act
        dialogsHistory.Add(dialogKey);
        dialogsHistory.Add(dialogKey); // Add same key again

        // Assert
        dialogsHistory.IsExist(dialogKey).Should().BeTrue();
    }

    [Fact]
    public void Add_Should_Handle_Empty_String()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var emptyKey = "";

        // Act
        dialogsHistory.Add(emptyKey);

        // Assert
        dialogsHistory.IsExist(emptyKey).Should().BeTrue();
    }

    [Fact]
    public void Add_Should_Handle_Null_String()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        string nullKey = null;

        // Act
        dialogsHistory.Add(nullKey);

        // Assert
        dialogsHistory.IsExist(nullKey).Should().BeTrue();
    }

    [Fact]
    public void IsExist_Should_Return_False_For_Non_Existent_Key()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var nonExistentKey = "non-existent-key";

        // Act
        var result = dialogsHistory.IsExist(nonExistentKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExist_Should_Return_False_For_Empty_History()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();

        // Act
        var result = dialogsHistory.IsExist("any-key");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExist_Should_Be_Case_Sensitive()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var key = "TestKey";

        // Act
        dialogsHistory.Add(key);

        // Assert
        dialogsHistory.IsExist(key).Should().BeTrue();
        dialogsHistory.IsExist("testkey").Should().BeFalse();
        dialogsHistory.IsExist("TESTKEY").Should().BeFalse();
        dialogsHistory.IsExist("TestKEY").Should().BeFalse();
    }

    [Fact]
    public void IsExist_Should_Handle_Null_Key()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        string nullKey = null;

        // Act
        var result = dialogsHistory.IsExist(nullKey);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExist_Should_Return_True_After_Adding_Null_Key()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        string nullKey = null;

        // Act
        dialogsHistory.Add(nullKey);
        var result = dialogsHistory.IsExist(nullKey);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Add_And_IsExist_Should_Handle_Special_Characters()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var specialKey = "dialog-with-special-chars!@#$%^&*()_+-={}[]|\\:;\"'<>?,./";

        // Act
        dialogsHistory.Add(specialKey);

        // Assert
        dialogsHistory.IsExist(specialKey).Should().BeTrue();
    }

    [Fact]
    public void Add_And_IsExist_Should_Handle_Whitespace_Keys()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var whitespaceKey = "   dialog with spaces   ";
        var tabKey = "\tdialog\twith\ttabs\t";
        var newlineKey = "dialog\nwith\nnewlines";

        // Act
        dialogsHistory.Add(whitespaceKey);
        dialogsHistory.Add(tabKey);
        dialogsHistory.Add(newlineKey);

        // Assert
        dialogsHistory.IsExist(whitespaceKey).Should().BeTrue();
        dialogsHistory.IsExist(tabKey).Should().BeTrue();
        dialogsHistory.IsExist(newlineKey).Should().BeTrue();
    }

    [Fact]
    public void Add_And_IsExist_Should_Handle_Long_Keys()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var longKey = new string('a', 1000); // 1000 character key

        // Act
        dialogsHistory.Add(longKey);

        // Assert
        dialogsHistory.IsExist(longKey).Should().BeTrue();
    }

    [Fact]
    public void DialogsHistory_Should_Maintain_State_Across_Operations()
    {
        // Arrange
        var dialogsHistory = new DialogsHistory();
        var keys = new[] { "key1", "key2", "key3", "key4", "key5" };

        // Act
        foreach (var key in keys)
        {
            dialogsHistory.Add(key);
        }

        // Check that all keys exist after adding
        foreach (var key in keys)
        {
            dialogsHistory.IsExist(key).Should().BeTrue($"Key {key} should exist");
        }

        // Add more keys
        var additionalKeys = new[] { "key6", "key7", "key8" };
        foreach (var key in additionalKeys)
        {
            dialogsHistory.Add(key);
        }

        // Assert - all original and additional keys should still exist
        foreach (var key in keys.Concat(additionalKeys))
        {
            dialogsHistory.IsExist(key).Should().BeTrue($"Key {key} should exist after additional operations");
        }
    }

    [Fact]
    public void DialogsHistory_Should_Be_Independent_Between_Instances()
    {
        // Arrange
        var history1 = new DialogsHistory();
        var history2 = new DialogsHistory();
        var key1 = "history1-key";
        var key2 = "history2-key";

        // Act
        history1.Add(key1);
        history2.Add(key2);

        // Assert
        history1.IsExist(key1).Should().BeTrue();
        history1.IsExist(key2).Should().BeFalse();
        
        history2.IsExist(key2).Should().BeTrue();
        history2.IsExist(key1).Should().BeFalse();
    }
}
