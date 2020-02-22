using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace YukiAPI.Hubs {
    public class PageviewHub : Hub {
        protected readonly ILogger<PageviewHub> Logger;
        public PageviewHub(ILogger<PageviewHub> logger) {
            Logger = logger;
        }

        public override async Task OnConnectedAsync() {
            Logger.LogInformation("PageviewHub Connected: {0}:{1}", Context.UserIdentifier, Context.ConnectionId);
            Context.Items["connectTime"] = DateTime.Now;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            Logger.LogInformation("PageviewHub Disconnected: {0}:{1}", Context.UserIdentifier, Context.ConnectionId);
            DateTime connTime = (DateTime)Context.Items["connectTime"];
            var duration = DateTime.Now - connTime;
            var url = Context.GetHttpContext().Request.Query["pageview-url"].ToString();
            Logger.LogInformation("PageviewHub Duration: {0}", duration);
            Logger.LogInformation("PageviewHub URL: {0}", url);

            Helper.SendEmail(new MailboxAddress("Pageview User", "yuki@yuki-nagato.com"), "新访客",
                "访客记录\n" +
                "访问页面: " + url + "\n" +
                "访问时间: " + connTime.ToString("O") + "\n" +
                "访问时长: " + duration.ToString("c") + "\n" +
                "客户IP: " + Context.GetHttpContext().Connection.RemoteIpAddress + "\n" +
                "Pageview请求头:\n" +
                Context.GetHttpContext().Request.Headers.ToHttpString()
            );

            await base.OnDisconnectedAsync(exception);
        }
    }
}