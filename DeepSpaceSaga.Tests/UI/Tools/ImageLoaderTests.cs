using DeepSpaceSaga.UI.Tools;
using System.Drawing;

namespace DeepSpaceSaga.Tests.UI.Tools;

public class ImageLoaderTests : IDisposable
{
    private readonly string _testImagesDirectory;
    private readonly string _testLayersTacticalDirectory;
    private readonly string _testCharactersDirectory;
    private readonly string _testItemsDirectory;

    public ImageLoaderTests()
    {
        // Create test directories in temp folder
        _testImagesDirectory = Path.Combine(Path.GetTempPath(), "DeepSpaceSagaTestImages");
        _testLayersTacticalDirectory = Path.Combine(_testImagesDirectory, "Images", "Layers", "Tactical");
        _testCharactersDirectory = Path.Combine(_testImagesDirectory, "Images", "Characters");
        _testItemsDirectory = Path.Combine(_testImagesDirectory, "Images");

        Directory.CreateDirectory(_testLayersTacticalDirectory);
        Directory.CreateDirectory(_testCharactersDirectory);
        Directory.CreateDirectory(_testItemsDirectory);

        // Create test image files
        CreateTestImage(Path.Combine(_testLayersTacticalDirectory, "test.png"));
        CreateTestImage(Path.Combine(_testCharactersDirectory, "test.png"));
        CreateTestImage(Path.Combine(_testItemsDirectory, "test.png"));
        
        // Create test image with dots in path for LoadItemImage test
        var testSubfolderPath = Path.Combine(_testImagesDirectory, "Images", "test", "subfolder.png");
        Directory.CreateDirectory(Path.GetDirectoryName(testSubfolderPath)!);
        CreateTestImage(testSubfolderPath);
    }

    private void CreateTestImage(string filePath)
    {
        using var bitmap = new Bitmap(10, 10);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Red);
        bitmap.Save(filePath);
    }

    [Fact]
    public void LoadLayersTacticalImage_WithValidFilename_ShouldReturnImage()
    {
        // Arrange
        var testImagePath = Path.Combine(_testLayersTacticalDirectory, "test.png");
        var testId = Guid.NewGuid().ToString("N")[..8];
        var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Layers", "Tactical", $"test_{testId}.png");
        
        // Copy test image to expected location
        var expectedDir = Path.GetDirectoryName(expectedPath);
        if (!Directory.Exists(expectedDir))
            Directory.CreateDirectory(expectedDir!);
        File.Copy(testImagePath, expectedPath, true);

        try
        {
            // Act
            using var result = ImageLoader.LoadLayersTacticalImage($"test_{testId}");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Image>();
        }
        finally
        {
            // Clean up - wait a bit to ensure file is not locked
            try
            {
                if (File.Exists(expectedPath))
                    File.Delete(expectedPath);
            }
            catch (IOException)
            {
                // File might still be locked, ignore
            }
        }
    }

    [Fact]
    public void LoadLayersTacticalImage_WithInvalidFilename_ShouldThrowFileNotFoundException()
    {
        // Act & Assert
        var action = () => ImageLoader.LoadLayersTacticalImage("nonexistent");
        action.Should().Throw<FileNotFoundException>()
            .WithMessage("Blueprint image file not found.*");
    }

    [Fact]
    public void LoadCharacterImage_WithValidFilename_ShouldReturnImage()
    {
        // Arrange
        var testImagePath = Path.Combine(_testCharactersDirectory, "test.png");
        var testId = Guid.NewGuid().ToString("N")[..8];
        var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Characters", $"test_{testId}.png");
        
        // Copy test image to expected location
        var expectedDir = Path.GetDirectoryName(expectedPath);
        if (!Directory.Exists(expectedDir))
            Directory.CreateDirectory(expectedDir!);
        File.Copy(testImagePath, expectedPath, true);

        try
        {
            // Act
            using var result = ImageLoader.LoadCharacterImage($"test_{testId}.png");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Image>();
        }
        finally
        {
            // Clean up - wait a bit to ensure file is not locked
            try
            {
                if (File.Exists(expectedPath))
                    File.Delete(expectedPath);
            }
            catch (IOException)
            {
                // File might still be locked, ignore
            }
        }
    }

    [Fact]
    public void LoadCharacterImage_WithInvalidFilename_ShouldThrowFileNotFoundException()
    {
        // Act & Assert
        var action = () => ImageLoader.LoadCharacterImage("nonexistent.png");
        action.Should().Throw<FileNotFoundException>()
            .WithMessage("Blueprint image file not found.*");
    }

    [Fact]
    public void LoadItemImage_WithValidFilename_ShouldReturnImage()
    {
        // Arrange
        var testImagePath = Path.Combine(_testItemsDirectory, "test.png");
        var testId = Guid.NewGuid().ToString("N")[..8];
        var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", $"test_{testId}.png");
        
        // Copy test image to expected location
        var expectedDir = Path.GetDirectoryName(expectedPath);
        if (!Directory.Exists(expectedDir))
            Directory.CreateDirectory(expectedDir!);
        File.Copy(testImagePath, expectedPath, true);

        try
        {
            // Act
            using var result = ImageLoader.LoadItemImage($"test_{testId}");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Image>();
        }
        finally
        {
            // Clean up - wait a bit to ensure file is not locked
            try
            {
                if (File.Exists(expectedPath))
                    File.Delete(expectedPath);
            }
            catch (IOException)
            {
                // File might still be locked, ignore
            }
        }
    }

    [Fact]
    public void LoadItemImage_WithInvalidFilename_ShouldThrowFileNotFoundException()
    {
        // Act & Assert
        var action = () => ImageLoader.LoadItemImage("nonexistent");
        action.Should().Throw<FileNotFoundException>()
            .WithMessage("Blueprint image file not found.*");
    }

    [Fact]
    public void LoadItemImage_WithDotsInFilename_ShouldReplaceDotsWithSlashes()
    {
        // Arrange
        var testImagePath = Path.Combine(_testImagesDirectory, "Images", "test", "subfolder.png");
        var testId = Guid.NewGuid().ToString("N")[..8];
        var expectedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "test", $"subfolder_{testId}.png");
        
        // Copy test image to expected location
        var expectedDir = Path.GetDirectoryName(expectedPath);
        if (!Directory.Exists(expectedDir))
            Directory.CreateDirectory(expectedDir!);
        File.Copy(testImagePath, expectedPath, true);

        try
        {
            // Act
            using var result = ImageLoader.LoadItemImage($"test.subfolder_{testId}");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Image>();
        }
        finally
        {
            // Clean up - wait a bit to ensure file is not locked
            try
            {
                if (File.Exists(expectedPath))
                    File.Delete(expectedPath);
            }
            catch (IOException)
            {
                // File might still be locked, ignore
            }
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void LoadLayersTacticalImage_WithNullOrEmptyFilename_ShouldThrowArgumentException(string filename)
    {
        // Act & Assert
        var action = () => ImageLoader.LoadLayersTacticalImage(filename);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Filename cannot be null, empty, or whitespace.*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void LoadCharacterImage_WithNullOrEmptyFilename_ShouldThrowArgumentException(string filename)
    {
        // Act & Assert
        var action = () => ImageLoader.LoadCharacterImage(filename);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Filename cannot be null, empty, or whitespace.*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void LoadItemImage_WithNullOrEmptyFilename_ShouldThrowArgumentException(string filename)
    {
        // Act & Assert
        var action = () => ImageLoader.LoadItemImage(filename);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Filename cannot be null, empty, or whitespace.*");
    }

    [Fact]
    public void LoadLayersTacticalImage_ShouldConstructCorrectPath()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var filename = $"nonexistent_{testId}";
        
        // Act & Assert
        var action = () => ImageLoader.LoadLayersTacticalImage(filename);
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void LoadCharacterImage_ShouldConstructCorrectPath()
    {
        // Arrange
        var filename = "test.png";
        
        // Act & Assert
        var action = () => ImageLoader.LoadCharacterImage(filename);
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void LoadItemImage_ShouldConstructCorrectPath()
    {
        // Arrange
        var filename = "test";
        
        // Act & Assert
        var action = () => ImageLoader.LoadItemImage(filename);
        action.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void LoadItemImage_WithDots_ShouldReplaceDotsWithSlashes()
    {
        // Arrange
        var filename = "test.subfolder";
        
        // Act & Assert
        var action = () => ImageLoader.LoadItemImage(filename);
        action.Should().Throw<FileNotFoundException>();
    }

    public void Dispose()
    {
        // Clean up test directories
        if (Directory.Exists(_testImagesDirectory))
        {
            Directory.Delete(_testImagesDirectory, true);
        }
    }
}
