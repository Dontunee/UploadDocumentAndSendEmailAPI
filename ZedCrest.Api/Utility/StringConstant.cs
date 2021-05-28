using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Utility
{
    public class StringConstant
    {
        public const string PhoneNumberRegex = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";

       

    }


    public class ApiResponses
    {

        public const string FileSizeExceeded = "One or more files exceeds maximum allowed limit";

        public const string UserDoesNotExist = "The user does not exist"; 
        
        public const string ErrorOccurred = "Error Occurred";


    }
}
