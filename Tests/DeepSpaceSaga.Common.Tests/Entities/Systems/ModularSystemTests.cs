using DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Cargo;
using DeepSpaceSaga.Common.Abstractions.Entities.Systems;
using DeepSpaceSaga.Common.Implementation.Entities.Systems;

namespace DeepSpaceSaga.Common.Tests.Entities.Systems;

public class ModularSystemTests
{
    private readonly ModularSystem _sut;

    public ModularSystemTests()
    {
        _sut = new ModularSystem();
    }

    #region Add Method Tests

    [Fact]
    public void Add_ShouldAddModuleToSystem()
    {
        // Arrange
        var module = CreateMockModule(1, "TestModule", Category.Weapon);

        // Act
        _sut.Add(module.Object);

        // Assert
        var modules = _sut.GetAll();
        modules.Should().HaveCount(1);
        modules.Should().Contain(module.Object);
    }

    [Fact]
    public void Add_ShouldAddMultipleModules()
    {
        // Arrange
        var module1 = CreateMockModule(1, "Module1", Category.Weapon);
        var module2 = CreateMockModule(2, "Module2", Category.Shield);

        // Act
        _sut.Add(module1.Object);
        _sut.Add(module2.Object);

        // Assert
        var modules = _sut.GetAll();
        modules.Should().HaveCount(2);
        modules.Should().Contain(module1.Object);
        modules.Should().Contain(module2.Object);
    }

    [Fact]
    public void Add_ShouldAllowSameModuleMultipleTimes()
    {
        // Arrange
        var module = CreateMockModule(1, "TestModule", Category.Weapon);

        // Act
        _sut.Add(module.Object);
        _sut.Add(module.Object);

        // Assert
        var modules = _sut.GetAll();
        modules.Should().HaveCount(2);
        modules.Should().AllBeEquivalentTo(module.Object);
    }

    #endregion

    #region Get Method Tests

    [Fact]
    public void Get_WithExistingModuleId_ShouldReturnModule()
    {
        // Arrange
        var module = CreateMockModule(1, "TestModule", Category.Weapon);
        _sut.Add(module.Object);

        // Act
        var result = _sut.Get(1);

        // Assert
        result.Should().Be(module.Object);
    }

    [Fact]
    public void Get_WithNonExistingModuleId_ShouldReturnNull()
    {
        // Arrange
        var module = CreateMockModule(1, "TestModule", Category.Weapon);
        _sut.Add(module.Object);

        // Act
        var result = _sut.Get(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Get_WithEmptySystem_ShouldReturnNull()
    {
        // Act
        var result = _sut.Get(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Get_WithMultipleModules_ShouldReturnCorrectModule()
    {
        // Arrange
        var module1 = CreateMockModule(1, "Module1", Category.Weapon);
        var module2 = CreateMockModule(2, "Module2", Category.Shield);
        var module3 = CreateMockModule(3, "Module3", Category.Reactor);
        _sut.Add(module1.Object);
        _sut.Add(module2.Object);
        _sut.Add(module3.Object);

        // Act
        var result = _sut.Get(2);

        // Assert
        result.Should().Be(module2.Object);
    }

    #endregion

    #region GetAll Method Tests

    [Fact]
    public void GetAll_WithEmptySystem_ShouldReturnEmptyList()
    {
        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_WithModules_ShouldReturnAllModules()
    {
        // Arrange
        var module1 = CreateMockModule(1, "Module1", Category.Weapon);
        var module2 = CreateMockModule(2, "Module2", Category.Shield);
        _sut.Add(module1.Object);
        _sut.Add(module2.Object);

        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(module1.Object);
        result.Should().Contain(module2.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnMutableList()
    {
        // Arrange
        var module = CreateMockModule(1, "TestModule", Category.Weapon);
        _sut.Add(module.Object);

        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().BeAssignableTo<List<IModule>>();
    }

    #endregion

    #region GetByCategory Method Tests

    [Fact]
    public void GetByCategory_WithExistingCategory_ShouldReturnModulesOfThatCategory()
    {
        // Arrange
        var weaponModule1 = CreateMockModule(1, "Weapon1", Category.Weapon);
        var weaponModule2 = CreateMockModule(2, "Weapon2", Category.Weapon);
        var shieldModule = CreateMockModule(3, "Shield1", Category.Shield);
        
        _sut.Add(weaponModule1.Object);
        _sut.Add(weaponModule2.Object);
        _sut.Add(shieldModule.Object);

        // Act
        var result = _sut.GetByCategory(Category.Weapon);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(weaponModule1.Object);
        result.Should().Contain(weaponModule2.Object);
        result.Should().NotContain(shieldModule.Object);
    }

    [Fact]
    public void GetByCategory_WithNonExistingCategory_ShouldReturnEmptyList()
    {
        // Arrange
        var weaponModule = CreateMockModule(1, "Weapon1", Category.Weapon);
        _sut.Add(weaponModule.Object);

        // Act
        var result = _sut.GetByCategory(Category.Shield);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetByCategory_WithEmptySystem_ShouldReturnEmptyList()
    {
        // Act
        var result = _sut.GetByCategory(Category.Weapon);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region Module Method Tests

    [Fact]
    public void Module_WithExistingCategory_ShouldReturnFirstModuleOfCategory()
    {
        // Arrange
        var weaponModule1 = CreateMockModule(1, "Weapon1", Category.Weapon);
        var weaponModule2 = CreateMockModule(2, "Weapon2", Category.Weapon);
        var shieldModule = CreateMockModule(3, "Shield1", Category.Shield);
        
        _sut.Add(weaponModule1.Object);
        _sut.Add(weaponModule2.Object);
        _sut.Add(shieldModule.Object);

        // Act
        var result = _sut.Module(Category.Weapon);

        // Assert
        result.Should().Be(weaponModule1.Object);
    }

    [Fact]
    public void Module_WithNonExistingCategory_ShouldReturnNull()
    {
        // Arrange
        var weaponModule = CreateMockModule(1, "Weapon1", Category.Weapon);
        _sut.Add(weaponModule.Object);

        // Act
        var result = _sut.Module(Category.Shield);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Module_WithEmptySystem_ShouldReturnNull()
    {
        // Act
        var result = _sut.Module(Category.Weapon);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetCoreContainer Property Tests

    [Fact]
    public void GetCoreContainer_ShouldBeSettable()
    {
        // Arrange
        var mockContainer = new Mock<ICargoContainer>();

        // Act
        _sut.GetCoreContainer = mockContainer.Object;

        // Assert
        _sut.GetCoreContainer.Should().Be(mockContainer.Object);
    }

    [Fact]
    public void GetCoreContainer_ShouldInitiallyBeNull()
    {
        // Act & Assert
        _sut.GetCoreContainer.Should().BeNull();
    }

    #endregion

    #region Helper Methods

    private Mock<IModule> CreateMockModule(int id, string name, Category category)
    {
        var mock = new Mock<IModule>();
        mock.Setup(m => m.Id).Returns(id);
        mock.Setup(m => m.Name).Returns(name);
        mock.Setup(m => m.Category).Returns(category);
        return mock;
    }

    #endregion
}
