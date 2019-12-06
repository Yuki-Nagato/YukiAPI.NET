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

namespace YukiAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // services.AddRouting((routeOptions) => {
            //     routeOptions.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            // });
            services.AddControllers();
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

            app.UseYuki();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }

    // public class SlugifyParameterTransformer : IOutboundParameterTransformer {
    //     public string TransformOutbound(object value) {
    //         // Slugify value
    //         return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
    //     }
    // }
}
