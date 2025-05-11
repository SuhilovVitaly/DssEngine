using log4net.Config;

namespace DeepSpaceSaga.Controller;

public static class Log4NetInitializer
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(Log4NetInitializer));

    public static void Initialize()
    {
        try
        {
            // Configure log4net
            var configFile = new FileInfo("log4net.config");
            XmlConfigurator.Configure(configFile);

            // Test log message
            Logger.Info("DeepSpaceSaga.Controller logging initialized");
        }
        catch (Exception ex)
        {
            // If we can't log to file, at least write to debug output
            Debug.WriteLine($"Failed to initialize logging: {ex.Message}");
            throw;
        }
    }
} 