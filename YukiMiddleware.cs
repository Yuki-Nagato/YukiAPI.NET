using System.Net;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace YukiAPI {
    public class YukiMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<YukiMiddleware> _logger;

        public YukiMiddleware(RequestDelegate next, ILogger<YukiMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
            string log = "Request Received:" + Environment.NewLine;
            log += string.Format("{0} {1}://{2}{3}{4} {5}",
                context.Request.Method,
                context.Request.Scheme,
                context.Request.Host,
                context.Request.Path,
                context.Request.QueryString,
                context.Request.Protocol
            ) + Environment.NewLine;
            log += string.Format("Client: {0}:{1}, Local: {2}:{3}",
                context.Connection.RemoteIpAddress,
                context.Connection.RemotePort,
                context.Connection.LocalIpAddress,
                context.Connection.LocalPort
            ) + Environment.NewLine;
            log += "Request Headers:" + Environment.NewLine;
            log += context.Request.Headers.ToHttpString();
            _logger.LogDebug(log);

            // 危险地使用CORS
            if (context.Request.Headers.ContainsKey("Origin")) {
                context.Response.Headers["Access-Control-Allow-Origin"] = context.Request.Headers["Origin"];
                context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                if (context.Request.Headers.ContainsKey("Access-Control-Request-Headers")) {
                    context.Response.Headers["Access-Control-Allow-Headers"] = context.Request.Headers["Access-Control-Request-Headers"];
                }
                if (context.Request.Headers.ContainsKey("Access-Control-Request-Method")) {
                    context.Response.Headers["Access-Control-Allow-Methods"] = context.Request.Headers["Access-Control-Request-Method"];
                }
            }

            await _next(context);

            if (context.Request.Headers.ContainsKey("Origin") && context.Request.Method == HttpMethod.Options.Method) {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }
    }
    public static class YukiMiddlewareExtensions {
        public static IApplicationBuilder UseYuki(this IApplicationBuilder builder) {
            return builder.UseMiddleware<YukiMiddleware>();
        }
    }
}