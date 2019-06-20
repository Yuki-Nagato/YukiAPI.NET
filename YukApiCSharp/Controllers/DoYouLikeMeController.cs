using System;
using Microsoft.AspNetCore.Mvc;

namespace YukApiCSharp.Controllers {
    [ApiController]
    public class DoYouLikeMeController : ControllerBase
    {
        [HttpPost("api/do-you-like-me")]
        public DoYouLikeMeResponse Query([FromBody] DoYouLikeMeRequest req, [FromHeader] string referer) {
            string host;
            if(req.host!=null) {
                host = req.host;
            }
            else {
                host = new Uri(referer).Host;
            }
            if(req.method=="get") {
                using (var conn = new DatabaseCommandConnection("select count(*) from do_you_like_me where host=@host")) {
                    conn.Command.Parameters.AddWithValue("host", host);
                    int cnt = (int)(long)conn.Command.ExecuteScalar();
                    return new DoYouLikeMeResponse() {
                        host = host,
                        count = cnt
                    };
                }
            }
            else if(req.method=="add") {
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
                        host = host,
                        count = cnt
                    };
                }
            }
            else {
                throw new ArgumentException("method必须为get或add");
            }
        }
    }

    public struct DoYouLikeMeRequest {
        public string method;
        public string host;
    }

    public struct DoYouLikeMeResponse {
        public string host;
        public int count;
    }
}