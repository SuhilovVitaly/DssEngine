using System.IO;
using log4net.Config;

namespace DeepSpaceSaga.Server;

public static class Settings
{
    public const string LoggerRepository = "ServerAppRepository";

    public static void ConfigureLogging()
    {
        var logRepository = LogManager.GetRepository(Settings.LoggerRepository);
        var configFile = new FileInfo("log4net.config");
        XmlConfigurator.Configure(logRepository, configFile);
    }
}