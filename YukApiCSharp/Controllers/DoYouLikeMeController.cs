using System;
using Microsoft.AspNetCore.Mvc;

namespace YukApiCSharp.Controllers {
    [ApiController]
    public class DoYouLikeMeController : YukiController {
        [HttpPost("api/do-you-like-me")]
        public DoYouLikeMeResponse Query([FromBody] DoYouLikeMeRequest req, [FromHeader] string referer) {
            string host = req.Host ?? new Uri(referer).Host;
            switch (req.Method) {
                case "get": {
                        using (var conn = new DatabaseCommandConnection("select count(*) from do_you_like_me where host=@host")) {
                            conn.Command.Parameters.AddWithValue("host", host);
                            int cnt = (int)(long)conn.Command.ExecuteScalar();
                            return new DoYouLikeMeResponse() {
                                Host = host,
                                Count = cnt
                            };
                        }
                    }

                case "add": {
                        using (var conn = new DatabaseCommandConnection("insert into do_you_like_me (host, req_headers, time) values (@host, @reqHeaders, @time)")) {
                            conn.Command.Parameters.AddWithValue("host", host);
                            conn.Command.Parameters.AddWithValue("reqHeaders", Tools.HeadersToString(Request.Headers));
                            conn.Command.Parameters.AddWithValue("time", DateTime.Now);
                            conn.Command.ExecuteNonQuery();
                        }

                        using (var conn = new DatabaseCommandConnection("select count(*) from do_you_like_me where host=@host")) {
                            conn.Command.Parameters.AddWithValue("host", host);
                            int cnt = (int)(long)conn.Command.ExecuteScalar();
                            return new DoYouLikeMeResponse() {
                                Host = host,
                                Count = cnt
                            };
                        }
                    }

                default:
                    throw new ArgumentException("method必须为get或add");
            }
        }
    }

    public struct DoYouLikeMeRequest {
        public string Method;
        public string Host;
    }

    public struct DoYouLikeMeResponse {
        public string Host;
        public int Count;
    }
}