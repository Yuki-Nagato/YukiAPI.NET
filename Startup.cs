using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YukiAPI.Hubs;

namespace YukiAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
            if (IntPtr.Size < 8) {
                throw new Exception("Must Run on x64 Platform");
            }
#if DEBUG
            logger.LogWarning("Built with Debug configuration.");
#else
            logger.LogInformation("Built with Release configuration.");
#endif
            if (env.IsProduction()) {
                logger.LogInformation("Running in Production environment.");
            }
            else {
                logger.LogWarning("Running in {0} environment.", env.EnvironmentName);
            }

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseYuki(); // 危险地使用CORS

            app.UseStaticFiles();

            app.UseRouting();


            // 由Yuki处理
            // app.UseCors((corsPolicyBuilder) => {
            //     corsPolicyBuilder.WithOrigins("http://localhost:81").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            // }); // CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapHub<PageviewHub>("/hub/pageview", (httpConnectionDispatcherOptions) => {
                });
                endpoints.MapControllers();
            });
        }
    }
}
