using DeepSpaceSaga.Server;

namespace DeepSpaceSaga.UI
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

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
            
            // Configure log4net
            var configFile = new FileInfo("log4net.config");
            log4net.Config.XmlConfigurator.Configure(configFile);

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