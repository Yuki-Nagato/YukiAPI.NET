using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YukApiCSharp {
    public class Program {
        public static int Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            if (IntPtr.Size < 8) {
                Console.WriteLine("Need x64 Profile");
                Environment.Exit(1);
            }
            Console.WriteLine($"Running as: {Environment.UserName}");
            Console.WriteLine("Args:");
            foreach (string arg in args) {
                Console.WriteLine(arg);
            }
            Console.WriteLine("GetEnvironmentVariables:");
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                Console.WriteLine("  {0} = {1}", de.Key, de.Value);
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}