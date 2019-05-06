using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Client1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Client1";

            var host = WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
                .UseEnvironment(Environment)
                .UseUrls("http://*:5005")
                .Build();

            host.Run();
        }

        private static string Environment
        {
            get
            {
                var path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Environment.txt";

                if (!File.Exists(path))
                {
                    path = $"{Directory.GetCurrentDirectory()}\\Environment.txt";

                    if (!File.Exists(path)) return "Development";

                }

                return File.ReadAllText(path);
            }
        }
    }
}
