namespace DeepSpaceSaga.Server.Tests;

public static class TestLoggerRepository
{
    private static readonly object _lock = new object();
    private static readonly string _configPath;
    private static bool _initialized = false;

    static TestLoggerRepository()
    {
        _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
        File.WriteAllText(_configPath, @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <log4net>
    <root>
      <level value=""ALL"" />
      <appender-ref ref=""console"" />
    </root>
    <appender name=""console"" type=""log4net.Appender.ConsoleAppender"">
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date %level %logger - %message%newline"" />
      </layout>
    </appender>
  </log4net>
</configuration>");
    }

    public static void Initialize()
    {
        if (_initialized)
            return;

        lock (_lock)
        {
            if (_initialized)
                return;

            try 
            {
                // Create and register the server repository
                InitializeRepository("ServerAppRepository");
                
                // Create and register the controller repository
                InitializeRepository("ControllerAppRepository");

                _initialized = true;
            }
            catch (Exception)
            {
                // Ignore initialization errors in tests
            }
        }
    }

    private static void InitializeRepository(string repositoryName)
    {
        // Check if repository already exists
        if (!LogManager.GetAllRepositories().Any(r => r.Name == repositoryName))
        {
            var repository = LogManager.CreateRepository(repositoryName);
            
            // Configure the repository
            XmlConfigurator.Configure(repository, new FileInfo(_configPath));
        }
    }

    public static void Cleanup()
    {
        if (File.Exists(_configPath))
        {
            try
            {
                File.Delete(_configPath);
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }
    }
} 