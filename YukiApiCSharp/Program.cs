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

namespace YukiApiCSharp {
    public class Program {
        public static int Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            if (IntPtr.Size < 8) {
                Logger.Log("Need x64 Profile");
                Environment.Exit(1);
            }
            Logger.Log($"Running as: {Environment.UserName}");
            Logger.Log("Args:");
            foreach (string arg in args) {
                Logger.Log(arg);
            }
            Logger.Log("GetEnvironmentVariables:");
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                Logger.Log($"{de.Key}={de.Value}");
            return WebHost.CreateDefaultBuilder(args).UseLibuv().UseUrls(Config.ReadConfig().Server.Urls).UseStartup<Startup>();
        }
    }
}