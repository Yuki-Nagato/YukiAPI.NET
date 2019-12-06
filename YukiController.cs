using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace YukiAPI {
    public class YukiController<TSelf> : ControllerBase {
        protected readonly ILogger<TSelf> Logger;
        public YukiController(ILogger<TSelf> logger) {
            Logger = logger;
        }
    }

    public static class Helper {
        public static string ToHttpString(this IHeaderDictionary header) {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, StringValues> i in header) {
                foreach (string j in i.Value) {
                    sb.Append(i.Key).Append(": ").Append(j).Append("\r\n");
                }
            }
            return sb.ToString();
        }
    }
}