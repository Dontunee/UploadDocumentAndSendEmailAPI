using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Models;

namespace ZedCrest.Api.Query
{
    public class GetUserInformationQuery : IRequest<ApiBaseResponse<UserInformationResponse>>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        [StringLength(128, ErrorMessage = "Reference cannot be more than 128 characters")]
        public string Reference { get; set; }
    }
}
