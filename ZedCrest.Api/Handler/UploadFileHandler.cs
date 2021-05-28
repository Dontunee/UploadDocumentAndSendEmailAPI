using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZedCrest.Api.Data;
using ZedCrest.Api.Models;
using ZedCrest.Api.Models.Entities;
using ZedCrest.Api.Utility;

namespace ZedCrest.Api.Handler

{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand,ApiBaseResponse<string[]>>
    {
        private readonly IConfiguration _configuration;
        private readonly ZedCrestContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UploadFileHandler> _logger;


        public UploadFileHandler(IConfiguration configuration, ZedCrestContext dbContext, IPublishEndpoint publishEndpoint, ILogger<UploadFileHandler> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiBaseResponse<string[]>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("UploadFileCommand is {@request}", request);

                var configPath = _configuration.GetValue<string>("FilePath");
                if (string.IsNullOrWhiteSpace(configPath))
                {
                    configPath = Directory.GetCurrentDirectory();
                }
                configPath = Path.Combine(configPath, "Uploads");

                if (request.Files != null && request.Files.Count > 0)
                {
                    if (!Directory.Exists(configPath))
                    {
                        Directory.CreateDirectory(configPath);
                    }

                    foreach (var file in request.Files)
                    {
                        var fileLength = _configuration.GetValue<int>("FileLength");
                        if (file.Length > fileLength)
                        {
                            return new ApiBaseResponse<string[]>()
                            {
                                Success = false,
                                Messages = new[]
                                 {
                                   ApiResponses.FileSizeExceeded
                            }
                            };
                        }

                        using (FileStream fs = new FileStream(Path.Combine(configPath, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fs, cancellationToken);
                        }

                    }
                }



                var insertedUser = await _dbContext.Users.AddAsync(new User()
                {
                    Email = request.Person.Email,
                    FirstName = request.Person.FirstName,
                    LastName = request.Person.LastName,
                    MobileNumber = request.Person.MobileNumber,
                });



                if (request.Files != null && request.Files.Count > 0)
                {
                    await _dbContext.ApplicationFiles.AddRangeAsync(request.Files.Select(x => new ApplicationFile()
                    {
                        Name = x.FileName,
                        Path = Path.Combine(configPath, x.FileName),
                        Owner = insertedUser.Entity,
                        ContentType = x.ContentType
                    }));
                }

                await _dbContext.SaveChangesAsync(cancellationToken);

                var references = await _dbContext.ApplicationFiles.Where(x => x.OwnerId == insertedUser.Entity.Id)
                                        .Select(x => x.Id).AsNoTracking().ToArrayAsync(cancellationToken);

                await _publishEndpoint.Publish<IFileSubmitted>(new
                {
                    UserId = insertedUser.Entity.Id,
                    Files = references
                });

                return new ApiBaseResponse<string[]>()
                {
                    Success = true,
                    Data = references
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Handle");
                return new ApiBaseResponse<string[]>()
                {
                    Success = false,
                    Messages = new[]
                    {
                        ApiResponses.ErrorOccurred
                    }
                };

            }
        }
    }
}
