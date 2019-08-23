using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace YukApiCSharp.Controllers {
    [ApiController]
    public class LoginController : YukiController {
        [HttpPost("api/login")]
        public LoginResponse Login([FromBody] LoginRequest req) {
            Console.WriteLine($"Login request: username: {req.Username}, password: {req.Password}");
            JObject session = GetYukiSession() ?? new JObject();
            JObject sessionUser = new JObject {
                ["username"] = req.Username
            };
            session["user"] = sessionUser;
            SetYukiSession(session);
            return new LoginResponse() {Username = req.Username};
        }

        [HttpPost("api/get-username")]
        public GetUsernameResponse GetUsername() {
            return new GetUsernameResponse() {
                Username = GetYukiSession()["user"]["username"].ToString()
            };
        }
    }

    public struct LoginRequest {
        public string Username;
        public string Password;
    }

    public struct LoginResponse {
        public string Username;
    }

    public struct GetUsernameResponse {
        public string Username;
    }
}