namespace DeepSpaceSaga.UI;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    // Get logger instance from the specific repository
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(Program));

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();

        // Create Logs directory if it doesn't exist
        Directory.CreateDirectory("Logs");

        // Configure log4net for WinForms project
        var winFormRepository = LogManager.CreateRepository(GeneralSettings.WinFormLoggerRepository);
        var configFile = new FileInfo("log4net.config");
        log4net.Config.XmlConfigurator.Configure(winFormRepository, configFile);

        // Configure log4net for Server project
        var serverRepository = LogManager.CreateRepository(GeneralSettings.ServerLoggerRepository);
        var serverConfigFile = new FileInfo("DeepSpaceSaga.Server/log4net.config");
        if (!serverConfigFile.Exists) serverConfigFile = new FileInfo("log4net.config.server");
        log4net.Config.XmlConfigurator.Configure(serverRepository, serverConfigFile);

        Logger.Info("Start 'Deep Space Saga' game desktop client.");

        ServiceProvider = CreateHostBuilder().Build().Services;

        ServiceProvider.GetRequiredService<IScreensService>();

        var mainForm = ServiceProvider.GetRequiredService<ScreenBackground>();
        Application.Run(mainForm);
    }

    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddCommonServices();
                services.AddClientScreens();
                services.AddClientServices();
                services.AddServerServices();                
            });
    }
}