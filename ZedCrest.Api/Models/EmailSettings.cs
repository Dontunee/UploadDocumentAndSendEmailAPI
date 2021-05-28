using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        public string From { get; set; }
    }
}
