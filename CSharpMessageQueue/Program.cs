namespace CSharpMessageQueue
{
    using System;
    using System.IO;
    using System.Reflection;
    using log4net;
    using log4net.Config;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.WindowsServices;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !Environment.Equals("Development");

            var pathToContentRoot = Directory.GetCurrentDirectory();

            if (isService)
            {
                pathToContentRoot = AppContext.BaseDirectory;
            }

            var configuration = GetConfiguration(pathToContentRoot, Environment);

            if (configuration.GetValue<bool>("EnableLog"))
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo($"{pathToContentRoot}\\log4net.config"));
            }
            
            var host = WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .UseEnvironment(Environment)
                .UseUrls(configuration.GetValue<string>("Host"))
                .Build();

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static string Environment
        {
            get
            {
                var path = $"{AppContext.BaseDirectory}\\Environment.txt";
                if (!File.Exists(path)) return "Development";
                return File.ReadAllText(path);
            }
        }

        private static IConfigurationRoot GetConfiguration(string contentRootPath, string environment)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(contentRootPath)
              .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
