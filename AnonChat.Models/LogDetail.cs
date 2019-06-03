using System;
using System.Collections.Generic;
using System.Text;

namespace AnonChat.Models
{
    public class LogDetail
    {
        public int Id { get; set; }
        public string RequestTime { get; set; }
        public string ResponseTime { get; set; }
        public string RequestUrl { get; set; }
        public string UserName { get; set; }
        public string Headers { get; set; }
        public string QueryString { get; set; }
        public string Body { get; set; }
        public string HttpVerb { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
    }
}
