using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrest.Api.Models
{
    public class UploadFileCommand : IRequest<ApiBaseResponse<string[]>>
    {
        [Required]
        public Person Person { get; set; }

        [Required]

        public IFormFileCollection  Files { get; set; }



    }
}
