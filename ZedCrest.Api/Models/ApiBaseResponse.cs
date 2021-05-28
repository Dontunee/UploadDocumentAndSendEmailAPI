using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models
{
    public class ApiBaseResponse
    {
        public bool Success { get; set; }

        public string[] Messages { get; set; }

    }

    public class ApiBaseResponse<T> : ApiBaseResponse
    {
        public T Data { get; set; }

    }
}
