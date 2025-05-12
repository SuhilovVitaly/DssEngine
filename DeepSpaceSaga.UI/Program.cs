using DeepSpaceSaga.Server;

namespace DeepSpaceSaga.UI
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        // Define a unique repository name
        private const string REPOSITORY_NAME = "UIApplicationRepository";

        // Get logger instance from the specific repository
        private static readonly ILog Logger = LogManager.GetLogger(REPOSITORY_NAME, typeof(Program));

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
            
            // Configure log4net for a specific repository
            var repository = LogManager.CreateRepository(REPOSITORY_NAME);
            var configFile = new FileInfo("log4net.config");
            log4net.Config.XmlConfigurator.Configure(repository, configFile);

            Logger.Info("Start 'Deep Space Saga' game desktop client.");

            ServiceProvider = CreateHostBuilder().Build().Services;

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddClientControls();
                    services.AddControllerServices();
                    services.AddServerServices();
                });
        }
    }
}