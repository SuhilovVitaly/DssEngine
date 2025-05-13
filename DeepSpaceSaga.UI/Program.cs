using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Implementation.Services;
using DeepSpaceSaga.Controller;
using DeepSpaceSaga.Server;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        // Create Logs directory if it doesn't exist
        Directory.CreateDirectory("Logs");

        // Configure log4net for Console project
        var consoleRepository = LogManager.CreateRepository(GeneralSettings.WinFormLoggerRepository);
        var consoleConfigFile = new FileInfo("log4net.config"); // Assumes log4net.config is in Console's output
        log4net.Config.XmlConfigurator.Configure(consoleRepository, consoleConfigFile);

        // Configure log4net for Controller project
        var controllerRepository = LogManager.CreateRepository(GeneralSettings.ControllerLoggerRepository);
        // Assuming DeepSpaceSaga.Controller/log4net.config is copied to output directory or accessible
        var controllerConfigFile = new FileInfo("DeepSpaceSaga.Controller/log4net.config");
        if (!controllerConfigFile.Exists) controllerConfigFile = new FileInfo("log4net.config.controller"); // Fallback or specific name
        log4net.Config.XmlConfigurator.Configure(controllerRepository, controllerConfigFile);

        // Configure log4net for Server project
        var serverRepository = LogManager.CreateRepository(GeneralSettings.ServerLoggerRepository);
        // Assuming DeepSpaceSaga.Server/log4net.config is copied to output directory or accessible
        var serverConfigFile = new FileInfo("DeepSpaceSaga.Server/log4net.config");
        if (!serverConfigFile.Exists) serverConfigFile = new FileInfo("log4net.config.server"); // Fallback or specific name
        log4net.Config.XmlConfigurator.Configure(serverRepository, serverConfigFile);

        Logger.Info("Start 'Deep Space Saga' game desktop client.");

        ServiceProvider = CreateHostBuilder().Build().Services;

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddScoped<SessionInfoService>();
                services.AddCommonServices();
                services.AddClientControls();
                services.AddControllerServices();
                services.AddServerServices();                
            });
    }
}