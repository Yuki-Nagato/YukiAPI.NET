using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace YukApiCSharp.Controllers
{
    [ApiController]
    public class LoginController : YukiController
    {
        [HttpPost("api/login")]
        public LoginResponse Login([FromBody] LoginRequest req) {
            Console.WriteLine($"Login request: username: {req.username}, password: {req.password}");
            JObject session = GetYukiSession();
            if (session == null)
                session = new JObject();
            JObject sessionUser = new JObject();
            sessionUser["username"] = req.username;
            session["user"] = sessionUser;
            SetYukiSession(session);
            return new LoginResponse() { username = req.username };
        }
        [HttpPost("api/get-username")]
        public GetUsernameResponse GetUsername() {
            return new GetUsernameResponse() {
                username = GetYukiSession()["user"]["username"].ToString()
            };
        }
    }

    public struct LoginRequest {
        public string username;
        public string password;
    }
    public struct LoginResponse {
        public string username;
    }
    public struct GetUsernameResponse {
        public string username;
    }
}