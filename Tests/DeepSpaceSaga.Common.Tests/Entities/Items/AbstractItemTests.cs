namespace DeepSpaceSaga.Common.Tests.Entities.Items;

public class AbstractItemTests
{
    [Fact]
    public void Constructor_Should_Initialize_Default_Values()
    {
        // Act
        var item = new AbstractItem();

        // Assert
        item.Id.Should().BeGreaterThan(0);
        item.Name.Should().BeNull();
        item.Image.Should().BeNull();
        item.OwnerId.Should().Be(0);
        item.Category.Should().Be(default);
        item.IsPacked.Should().BeFalse();
        item.Volume.Should().Be(0);
    }

    [Fact]
    public void Id_Should_Be_Unique_For_Different_Instances()
    {
        // Act
        var item1 = new AbstractItem();
        var item2 = new AbstractItem();

        // Assert
        item1.Id.Should().NotBe(item2.Id);
    }

    [Theory]
    [InlineData("Test Item")]
    [InlineData("")]
    [InlineData(null)]
    public void Name_Should_Set_And_Get_Correctly(string name)
    {
        // Arrange
        var item = new AbstractItem();

        // Act
        item.Name = name;

        // Assert
        item.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("test.png")]
    [InlineData("")]
    [InlineData(null)]
    public void Image_Should_Set_And_Get_Correctly(string image)
    {
        // Arrange
        var item = new AbstractItem();

        // Act
        item.Image = image;

        // Assert
        item.Image.Should().Be(image);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(123)]
    [InlineData(-1)]
    [InlineData(long.MaxValue)]
    public void OwnerId_Should_Set_And_Get_Correctly(long ownerId)
    {
        // Arrange
        var item = new AbstractItem();

        // Act
        item.OwnerId = ownerId;

        // Assert
        item.OwnerId.Should().Be(ownerId);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsPacked_Should_Set_And_Get_Correctly(bool isPacked)
    {
        // Arrange
        var item = new AbstractItem();

        // Act
        item.IsPacked = isPacked;

        // Assert
        item.IsPacked.Should().Be(isPacked);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.5)]
    [InlineData(100.0)]
    [InlineData(-1.0)]
    [InlineData(double.MaxValue)]
    public void Volume_Should_Set_And_Get_Correctly(double volume)
    {
        // Arrange
        var item = new AbstractItem();

        // Act
        item.Volume = volume;

        // Assert
        item.Volume.Should().Be(volume);
    }

    [Fact]
    public void Id_Should_Set_And_Get_Correctly()
    {
        // Arrange
        var item = new AbstractItem();
        var newId = 999;

        // Act
        item.Id = newId;

        // Assert
        item.Id.Should().Be(newId);
    }

    [Fact]
    public void All_Properties_Should_Be_Settable()
    {
        // Arrange
        var item = new AbstractItem();
        var testId = 123;
        var testName = "Test Item";
        var testImage = "test.png";
        var testOwnerId = 456L;
        var testCategory = Category.Weapon;
        var testIsPacked = true;
        var testVolume = 10.5;

        // Act
        item.Id = testId;
        item.Name = testName;
        item.Image = testImage;
        item.OwnerId = testOwnerId;
        item.Category = testCategory;
        item.IsPacked = testIsPacked;
        item.Volume = testVolume;

        // Assert
        item.Id.Should().Be(testId);
        item.Name.Should().Be(testName);
        item.Image.Should().Be(testImage);
        item.OwnerId.Should().Be(testOwnerId);
        item.Category.Should().Be(testCategory);
        item.IsPacked.Should().Be(testIsPacked);
        item.Volume.Should().Be(testVolume);
    }
} 