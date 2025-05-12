using DeepSpaceSaga.Controller;
using DeepSpaceSaga.Server;
using log4net;
using Microsoft.Extensions.Hosting;

namespace DeepSpaceSaga.Console
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        
       [STAThread]
       static void Main()
       {
           // Create Logs directory if it doesn't exist
           Directory.CreateDirectory("Logs");
           
           // Configure log4net
           var configFile = new FileInfo("log4net.config");
           log4net.Config.XmlConfigurator.Configure(configFile);

           Logger.Info("Start 'Deep Space Saga' game desktop client.");

           ServiceProvider = CreateHostBuilder().Build().Services;
           
           System.Console.WriteLine("Hello, World!");
       }
       
       static IHostBuilder CreateHostBuilder()
       {
           return Host.CreateDefaultBuilder()
               .ConfigureServices((_, services) => {
                   services.AddClientControls();
                   services.AddControllerServices();
                   services.AddServerServices();
               });
       }
    }

}

