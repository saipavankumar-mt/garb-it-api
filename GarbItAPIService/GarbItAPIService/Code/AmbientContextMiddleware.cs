using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbItAPIService.Code
{
    public class AmbientContextMiddleware : IMiddleware
    {
        private ISessionService _sessionService;
        private IAdminService _adminService;
        private IEmployeeService _employeeService;
        private AppSettings _settings;

        public AmbientContextMiddleware(ISessionService sessionService, IAdminService adminService, IEmployeeService employeeService, IOptions<AppSettings> options)
        {
            _sessionService = sessionService;
            _adminService = adminService;
            _employeeService = employeeService;
            _settings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {            
            var apiSessionId = string.Empty;

            if(context.Request.Path.Value.ToLower().Contains("login") || context.Request.Path.Value.ToLower().Contains("swagger"))
            {
                await next(context);
            }

            else if(context?.Request.Headers.ContainsKey("access-key") == true)
            {
                var accessKey = context.Request.Headers["access-key"];
                if(accessKey.Equals(_settings.AccessKey))
                {
                    await next(context);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            else if (context?.Request.Headers.ContainsKey("session-key") == true)
            {
                apiSessionId = context.Request.Headers["session-key"];
                
                using (var ambientContext = new AmbientContext(apiSessionId))
                {
                    var userProfile = await _sessionService.GetSessionInfoAsync(apiSessionId);

                    if (userProfile != null)
                    {
                        ambientContext.Role = userProfile.Role;
                        ambientContext.UserId = userProfile.UserId;
                        ambientContext.UserName = userProfile.UserName;
                        
                        await next(context);
                    }
                    else
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
