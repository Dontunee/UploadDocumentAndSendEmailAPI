using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models.Entities
{
    public class ApplicationFile
    {
        public string Id { get; set; }


        public string Name { get; set; }
        public string ContentType { get; set; }

        public string Path { get; set; }

        public User  Owner{ get; set; }

        public int OwnerId { get; set; }


    }
}
