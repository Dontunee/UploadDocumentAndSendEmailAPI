using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models
{
    public class FileSubmitted : IFileSubmitted
    {

        public int UserId { get; set; }

        public string[] Files { get; set; }

    }
}
