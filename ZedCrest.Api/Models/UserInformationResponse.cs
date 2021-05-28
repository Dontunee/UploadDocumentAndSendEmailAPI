using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models
{
    public class UserInformationResponse
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }


        public string[] FileUrls { get; set; }
    }
}
