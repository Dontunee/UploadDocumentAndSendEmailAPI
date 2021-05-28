using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZedCrest.Api.Data;
using ZedCrest.Api.Models;
using ZedCrest.Api.Query;
using ZedCrest.Api.Utility;

namespace ZedCrest.Api.Handler
{
    public class GetUserInformationHandler : IRequestHandler<GetUserInformationQuery, ApiBaseResponse<UserInformationResponse>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ZedCrestContext _dbContext;
        private readonly ILogger<GetUserInformationHandler> _logger;

        public GetUserInformationHandler(IHttpContextAccessor httpContextAccessor, ZedCrestContext dbContext, ILogger<GetUserInformationHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiBaseResponse<UserInformationResponse>> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("UserInformationQuery is {@request}", request);

                var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Email == request.Email);

                if (user is null)
                    return new ApiBaseResponse<UserInformationResponse>()
                    {
                        Messages = new string[]
                          {
                          ApiResponses.UserDoesNotExist
                          }
                    };

                var userFiles = await _dbContext.ApplicationFiles.Where(x => x.OwnerId == user.Id).AsNoTracking().ToListAsync();

                var host = _httpContextAccessor.HttpContext.Request.Host.Value;
                var scheme = _httpContextAccessor.HttpContext.Request.Scheme;

                var hostUrl = $"{scheme}://{host}/";

                return new ApiBaseResponse<UserInformationResponse>()
                {
                    Success = true,
                    Data = new UserInformationResponse()
                    {
                        EmailAddress = user.Email,
                        MobileNumber = user.MobileNumber,
                        Name = user.FirstName + " " + user.LastName,
                        FileUrls = userFiles.Select(x => $"{hostUrl}download-file?file={x.Id}").ToArray()
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Handle");
                return new ApiBaseResponse<UserInformationResponse>()
                {
                    Messages = new string[]
                        {
                          ApiResponses.ErrorOccurred
                        }
                };

            }

        }
           
    }
}
