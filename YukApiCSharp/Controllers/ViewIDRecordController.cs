using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace YukApiCSharp.Controllers {
    [ApiController]
    public class ViewIDRecordController : ControllerBase {
        [HttpPost("api/view-id-record")]
        public ViewIDRecordResponse Record([FromBody] ViewIDRecordRequest req) {
            Console.WriteLine($"收到View ID记录请求，View ID: {req.id}");
            using(var conn = new DatabaseCommandConnection("insert into view_id_record (view_id, req_headers, time) values (@id, @reqHeaders, @time)")) {
                if (req.id != null)
                    conn.Command.Parameters.AddWithValue("id", req.id);
                else
                    conn.Command.Parameters.AddWithValue("id", DBNull.Value);
                conn.Command.Parameters.AddWithValue("reqHeaders", Tools.HeadersToString(Request.Headers));
                conn.Command.Parameters.AddWithValue("time", DateTime.Now);
                conn.Command.ExecuteNonQuery();
            }
            return new ViewIDRecordResponse() {
                time = DateTime.Now
            };
        }
    }

    public struct ViewIDRecordRequest {
        public string id;
    }

    public struct ViewIDRecordResponse {
        public DateTime time;
    }
}