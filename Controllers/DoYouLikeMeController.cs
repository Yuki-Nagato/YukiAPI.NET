using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace YukiAPI.Controllers {
    [Route("api/do-you-like-me")]
    [ApiController]
    public class DoYouLikeMeController : YukiController<DoYouLikeMeController> {
        public DoYouLikeMeController(ILogger<DoYouLikeMeController> logger) : base(logger) {
            logger.LogDebug("DoYouLikeMeController Constructed");
        }

        [HttpPost]
        public DoYouLikeMeResponse Post([FromBody]DoYouLikeMeRequest req) {
            switch (req.Method) {
                case "add": {
                        Logger.LogInformation("DoYouLikeMe Add, Host={0}", req.Host);
                        List<DoYouLikeMeRecord> records;
                        using (var reader = new StreamReader("PrivateData/Database/DoYouLikeMe.csv"))
                        using (var csvReader = new CsvReader(reader)) {
                            records = csvReader.GetRecords<DoYouLikeMeRecord>().ToList();
                        }
                        records.Add(new DoYouLikeMeRecord() {
                            Host = req.Host,
                            Time = DateTime.Now,
                            Header = Request.Headers.ToHttpString()
                        });
                        using (var writer = new StreamWriter("PrivateData/Database/DoYouLikeMe.csv"))
                        using (var csvWriter = new CsvWriter(writer)) {
                            csvWriter.WriteRecords(records);
                        }
                        DoYouLikeMeResponse resp = new DoYouLikeMeResponse() {
                            Host = req.Host,
                            Count = 0
                        };
                        foreach (DoYouLikeMeRecord record in records) {
                            if (record.Host == req.Host) {
                                resp.Count++;
                            }
                        }
                        return resp;
                    }
                case "get": {
                        Logger.LogInformation("DoYouLikeMe Get, Host={0}", req.Host);
                        DoYouLikeMeResponse resp = new DoYouLikeMeResponse() {
                            Host = req.Host,
                            Count = 0
                        };
                        using (var reader = new StreamReader("PrivateData/Database/DoYouLikeMe.csv"))
                        using (var csvReader = new CsvReader(reader)) {
                            foreach (var record in csvReader.GetRecords<DoYouLikeMeRecord>()) {
                                if (record.Host == req.Host) {
                                    resp.Count++;
                                }
                            }
                        }
                        return resp;
                    }
                default: {
                        throw new ArgumentException();
                    }
            }
        }
        public struct DoYouLikeMeRequest {
            public string Host { get; set; }
            // public enum MethodType { get, add };
            // public MethodType Method { get; set; }
            public string Method { get; set; }
        }
        public struct DoYouLikeMeResponse {
            public string Host { get; set; }
            public int Count { get; set; }
        }

        public struct DoYouLikeMeRecord {
            public string Host { get; set; }
            public DateTime Time { get; set; }
            public string Header { get; set; }
        }
    }
}