using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace YukiAPI.Controllers {
    [ApiController]
    [Route("api/pageview")]
    public class PageviewController : YukiController<PageviewController> {
        public PageviewController(ILogger<PageviewController> logger) : base(logger) {
        }

        [HttpGet]
        public ContentResult Get() {
            string content = @"'use strict';

const signalrScript = document.createElement('script');  
signalrScript.setAttribute('src','https://cdn.jsdelivr.net/npm/@microsoft/signalr@3.0.1/dist/browser/signalr.js');
signalrScript.setAttribute('integrity', 'sha256-SUWjzIOkuKhBFIpipPTjTY0AIXJzcuIosJFjZt/aPPc=');
signalrScript.setAttribute('crossorigin', 'anonymous');

signalrScript.onload = function() {
  const connectionUrl = new URL('https://api.yuki-nagato.com/hub/pageview');
  connectionUrl.searchParams.set('pageview-url', location.href);
  
  const connection = new signalR.HubConnectionBuilder().withUrl(connectionUrl.href).build();
  
  connection.start();
}

document.head.appendChild(signalrScript);";

            return new ContentResult() {
                Content = content,
                ContentType = "application/javascript; charset=utf-8"
            };
        }
    }
}