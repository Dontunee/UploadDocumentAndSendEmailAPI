using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Utility;

namespace ZedCrest.Api.Models
{
    public class Person
    {
        [Required(ErrorMessage = "First name is required")]

        [StringLength(128, ErrorMessage ="First name cannot be more than 128 characters")]
        public string FirstName { get;  set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(128, ErrorMessage = "Last name cannot be more than 128 characters")]
        public string LastName { get;  set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get;  set; }

        [RegularExpression(StringConstant.PhoneNumberRegex, ErrorMessage = "This is not a valid phone number")]
        public string MobileNumber { get;  set; }

    }
}
