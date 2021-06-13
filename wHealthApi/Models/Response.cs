using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wHealthApi.Models
{
    public class Response
    {
        public bool Status { get; set; }
        public dynamic Result { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }
}
