using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Controller;
using DeepSpaceSaga.Server;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeepSpaceSaga.Console
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        // Define repository names
        private const string CONSOLE_REPOSITORY_NAME = "ConsoleAppRepository";
        private const string CONTROLLER_REPOSITORY_NAME = "ControllerAppRepository";
        private const string SERVER_REPOSITORY_NAME = "ServerAppRepository";

        // Get logger instance from the specific repository
        private static readonly ILog Logger = LogManager.GetLogger(CONSOLE_REPOSITORY_NAME, typeof(Program));
        
       [STAThread]
       static void Main()
       {
           // Create Logs directory if it doesn't exist
           Directory.CreateDirectory("Logs");
           
           // Configure log4net for Console project
           var consoleRepository = LogManager.CreateRepository(CONSOLE_REPOSITORY_NAME);
           var consoleConfigFile = new FileInfo("log4net.config"); // Assumes log4net.config is in Console's output
           log4net.Config.XmlConfigurator.Configure(consoleRepository, consoleConfigFile);

           // Configure log4net for Controller project
           var controllerRepository = LogManager.CreateRepository(CONTROLLER_REPOSITORY_NAME);
           // Assuming DeepSpaceSaga.Controller/log4net.config is copied to output directory or accessible
           var controllerConfigFile = new FileInfo("DeepSpaceSaga.Controller/log4net.config"); 
           if (!controllerConfigFile.Exists) controllerConfigFile = new FileInfo("log4net.config.controller"); // Fallback or specific name
           log4net.Config.XmlConfigurator.Configure(controllerRepository, controllerConfigFile);

           // Configure log4net for Server project
           var serverRepository = LogManager.CreateRepository(SERVER_REPOSITORY_NAME);
           // Assuming DeepSpaceSaga.Server/log4net.config is copied to output directory or accessible
           var serverConfigFile = new FileInfo("DeepSpaceSaga.Server/log4net.config"); 
           if (!serverConfigFile.Exists) serverConfigFile = new FileInfo("log4net.config.server"); // Fallback or specific name
           log4net.Config.XmlConfigurator.Configure(serverRepository, serverConfigFile);

           Logger.Info("Start 'Deep Space Saga' game desktop client.");

           ServiceProvider = CreateHostBuilder().Build().Services;
           
           var _worker = ServiceProvider.GetService<IGameServer>();
           _worker.SessionStart();
           
           System.Console.WriteLine("Hello, World!");
           System.Console.ReadLine();
       }
       
       static IHostBuilder CreateHostBuilder()
       {
           return Host.CreateDefaultBuilder()
               .ConfigureServices((_, services) => {
                   services.AddCommonServices();
                   services.AddClientControls();
                   services.AddServerServices();
               });
       }
    }

}

