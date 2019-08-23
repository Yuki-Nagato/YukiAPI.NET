using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace YukApiCSharp {
    public abstract class YukiController : ControllerBase {
        [NonAction]
        public JObject GetYukiSession() {
            if (!Request.Cookies.ContainsKey("YUKISESSION"))
                return null;
            string[] nonceAndCipher = Request.Cookies["YUKISESSION"].Split('.');
            byte[] nonce = Convert.FromBase64String(nonceAndCipher[0]);
            byte[] cipher = Convert.FromBase64String(nonceAndCipher[1]);
            string keyHex = Config.ReadConfig().Session.Key;
            byte[] key = Tools.HexToByteArray(keyHex);

            byte[] plain = Tools.AesGcm256Decrypt(cipher, key, nonce);
            string sessionStr = Encoding.UTF8.GetString(plain);
            JObject rst = JObject.Parse(sessionStr);
            Console.WriteLine("Session get: " + rst.ToString());
            return rst;
        }

        [NonAction]
        public void SetYukiSession(JObject value) {
            if (value == null) {
                Response.Cookies.Append("YUKISESSION", null, new CookieOptions() {MaxAge = TimeSpan.Zero});
                Console.WriteLine("Session clear");
                return;
            }

            string sessionStr = value.ToString(Newtonsoft.Json.Formatting.None);
            byte[] cipher, nonce;
            string keyHex = Config.ReadConfig().Session.Key;
            byte[] key = Tools.HexToByteArray(keyHex);

            cipher = Tools.AesGcm256Encrypt(Encoding.UTF8.GetBytes(sessionStr), key, out nonce);
            Response.Cookies.Append("YUKISESSION", Convert.ToBase64String(nonce) + "." + Convert.ToBase64String(cipher), new CookieOptions() {
                HttpOnly = true,
                MaxAge = TimeSpan.FromDays(2),
                Path = "/api/",
                SameSite = SameSiteMode.None
            });
            Console.WriteLine("Session set: " + value.ToString());
        }
    }
}