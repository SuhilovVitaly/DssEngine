using System.IO;
using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using DeepSpaceSaga.UI.Controller.Services.Localization;

namespace DeepSpaceSaga.UI.Controller.Tests.Services.Localization;

public class LocalizationServiceTests
{
    private sealed class TestSettings : ISettings
    {
        public int LanguageId { get; set; }
    }

    [Fact]
    public void Constructor_NullFolderPath_ThrowsArgumentNullException()
    {
        var settings = new TestSettings { LanguageId = 1 };

        Assert.Throws<ArgumentNullException>(() => new LocalizationService(settings, null!));
    }

    [Fact]
    public void Constructor_MissingDirectory_ThrowsDirectoryNotFoundException()
    {
        var settings = new TestSettings { LanguageId = 1 };
        var missingFolderName = $"LocMissing_{Guid.NewGuid():N}";

        Assert.Throws<DirectoryNotFoundException>(() => new LocalizationService(settings, missingFolderName));
    }

    [Fact]
    public void GetText_ReturnsLocalizedText_ForExistingKeyAndLanguage()
    {
        var folderName = $"LocOk_{Guid.NewGuid():N}";
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var targetDir = Path.Combine(baseDir, "Data", folderName);
        Directory.CreateDirectory(targetDir);

        try
        {
            var jsonContent = """
            {
              "LanguageId": 1,
              "texts": [
                { "Key": "HELLO", "Text": "Hello World" }
              ]
            }
            """;
            File.WriteAllText(Path.Combine(targetDir, "en.json"), jsonContent);

            var settings = new TestSettings { LanguageId = 1 };
            var service = new LocalizationService(settings, folderName);

            var message = service.GetText("HELLO");
            Assert.Equal("Hello World", message);
        }
        finally
        {
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, recursive: true);
            }
        }
    }

    [Fact]
    public void GetText_ReplacesEnvironmentNewLinePlaceholder()
    {
        var folderName = $"LocNewLine_{Guid.NewGuid():N}";
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var targetDir = Path.Combine(baseDir, "Data", folderName);
        Directory.CreateDirectory(targetDir);

        try
        {
            var jsonContent = """
            {
              "LanguageId": 2,
              "texts": [
                { "Key": "MULTI", "Text": "Line1{Environment.NewLine}Line2" }
              ]
            }
            """;
            File.WriteAllText(Path.Combine(targetDir, "ru.json"), jsonContent);

            var settings = new TestSettings { LanguageId = 2 };
            var service = new LocalizationService(settings, folderName);

            var message = service.GetText("MULTI");
            Assert.Equal($"Line1{Environment.NewLine}Line2", message);
        }
        finally
        {
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, recursive: true);
            }
        }
    }

    [Fact]
    public void GetText_ReturnsFallback_WhenTextNotFound()
    {
        var folderName = $"LocFallback_{Guid.NewGuid():N}";
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var targetDir = Path.Combine(baseDir, "Data", folderName);
        Directory.CreateDirectory(targetDir);

        try
        {
            var jsonContent = """
            {
              "LanguageId": 5,
              "texts": [
                { "Key": "KNOWN", "Text": "Known Text" }
              ]
            }
            """;
            File.WriteAllText(Path.Combine(targetDir, "custom.json"), jsonContent);

            var settings = new TestSettings { LanguageId = 5 };
            var service = new LocalizationService(settings, folderName);

            var notFound = service.GetText("UNKNOWN");
            Assert.Equal("Text not found for language 5 and text ID UNKNOWN", notFound);
        }
        finally
        {
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, recursive: true);
            }
        }
    }
}


