namespace DeepSpaceSaga.UI;

internal static class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }

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

        // Configure log4net for Controller project
        var controllerRepository = LogManager.CreateRepository(GeneralSettings.ControllerLoggerRepository);
        var controllerConfigFile = new FileInfo("DeepSpaceSaga.Controller/log4net.config.controller");
        if (!controllerConfigFile.Exists) controllerConfigFile = new FileInfo("log4net.config");
        log4net.Config.XmlConfigurator.Configure(controllerRepository, controllerConfigFile);

        // Configure log4net for Server project
        var serverRepository = LogManager.CreateRepository(GeneralSettings.ServerLoggerRepository);
        var serverConfigFile = new FileInfo("DeepSpaceSaga.Server/log4net.config.server");
        if (!serverConfigFile.Exists) serverConfigFile = new FileInfo("log4net.config");
        log4net.Config.XmlConfigurator.Configure(serverRepository, serverConfigFile);

        Logger.Info("Start 'Deep Space Saga' game desktop client.");

        ServiceProvider = CreateHostBuilder().Build().Services;

        ServiceProvider.GetRequiredService<IScreensService>();
        ServiceProvider.GetRequiredService<IGameEventsService>();

        var mainForm = ServiceProvider.GetRequiredService<ScreenBackground>();
        Application.Run(mainForm);
    }

    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddCommonServices();
                services.AddUiControllerServices();
                services.AddClientScreens();
                services.AddClientServices();
                services.AddServerServices();                
            });
    }
}