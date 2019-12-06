using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YukiAPI {
    public class Program {
        public static void Main(string[] args) {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseKestrel((webHostBuilderContext, kestrelServerOptions) => {
#if !YUKI_NO_HTTPS
                    X509Certificate2 cert = new X509Certificate2(Config.Read().Https.Certificate.File, Config.Read().Https.Certificate.Password);
                    kestrelServerOptions.ConfigureHttpsDefaults((httpsConnectionAdapterOptions) => {
                        httpsConnectionAdapterOptions.ServerCertificate = cert;
                    });
#endif
                    kestrelServerOptions.AddServerHeader = false;
                });
            });
            IHost host = hostBuilder.Build();
            host.Run();
        }
    }
}
