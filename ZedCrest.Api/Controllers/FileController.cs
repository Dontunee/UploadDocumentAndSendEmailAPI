using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Data;

namespace ZedCrest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ZedCrestContext _dbContext;

        public FileController(ZedCrestContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("/download-file")]
        public async Task<IActionResult> DownloadFileAsync([FromQuery] string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return BadRequest();
            }

            var result = await _dbContext.ApplicationFiles.FirstOrDefaultAsync(x => x.Id == file);

            if (result == null)
            {
                return NotFound();
            }

           var fileBytes =  await System.IO.File.ReadAllBytesAsync(result.Path);
           return File(fileBytes, "application/octet-stream", result.Name);

        }
    }
}
