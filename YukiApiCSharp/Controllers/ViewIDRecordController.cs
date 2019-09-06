using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace YukiApiCSharp.Controllers {
    [ApiController]
    public class ViewIDRecordController : ControllerBase {
        [HttpPost("api/view-id-record")]
        public ViewIDRecordResponse Record([FromBody] ViewIDRecordRequest req) {
            Logger.Log($"收到View ID记录请求，View ID: {req.Id}");
            using (var conn = new DatabaseCommandConnection("insert into view_id_record (view_id, req_headers, time) values (@id, @reqHeaders, @time)")) {
                if (req.Id != null)
                    conn.Command.Parameters.AddWithValue("id", req.Id);
                else
                    conn.Command.Parameters.AddWithValue("id", DBNull.Value);
                conn.Command.Parameters.AddWithValue("reqHeaders", Tools.HeadersToString(Request.Headers));
                conn.Command.Parameters.AddWithValue("time", DateTime.Now);
                conn.Command.ExecuteNonQuery();
            }

            return new ViewIDRecordResponse() {
                Time = DateTime.Now
            };
        }
    }

    public struct ViewIDRecordRequest {
        public string Id;
    }

    public struct ViewIDRecordResponse {
        public DateTime Time;
    }
}