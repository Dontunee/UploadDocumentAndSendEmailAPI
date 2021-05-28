using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Models;
using ZedCrest.Api.Query;

namespace ZedCrest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json", "multipart/form-data")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("/upload_file")]
        public async Task<IActionResult> UploadFileAsync([FromForm]Person person, [FromForm] IFormFileCollection formFiles)
        {
            var uploadRequest = new UploadFileCommand()
            {
                Person = person,
                Files = formFiles
            };

            var result =  await _mediator.Send(uploadRequest);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("/user")]
        public async Task<IActionResult> GetUserInfomationAsync([FromQuery] GetUserInformationQuery query)
        {
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
