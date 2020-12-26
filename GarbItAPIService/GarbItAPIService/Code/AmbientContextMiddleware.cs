using Contracts;
using Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public AmbientContextMiddleware(ISessionService sessionService, IAdminService adminService, IEmployeeService employeeService)
        {
            _sessionService = sessionService;
            _adminService = adminService;
            _employeeService = employeeService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {            
            var apiSessionId = string.Empty;

            if(context.Request.Path.Value.Contains("login"))
            {
                await next(context);
            }

            else if (context?.Request.Headers.ContainsKey("session-key") == true)
            {
                apiSessionId = context.Request.Headers["session-key"];
                
                using (var ambientContext = new AmbientContext(apiSessionId))
                {
                    var userProfile = await _sessionService.GetSessionInfoAsync(apiSessionId);

                    if (userProfile != null)
                    {
                        switch (userProfile.Role)
                        {
                            case Contracts.Models.Role.Admin:
                                {
                                    var adminInfo = await _adminService.GetAdminInfoAsync(userProfile.UserId);
                                    ambientContext.UserId = adminInfo.AdminId;
                                    ambientContext.UserName = adminInfo.UserName;
                                }
                                break;
                            case Contracts.Models.Role.Employee:
                                {
                                    var employeeInfo = await _employeeService.GetEmployeeInfoAsync(userProfile.UserId);
                                    ambientContext.UserId = employeeInfo.EmployeeId;
                                    ambientContext.UserName = employeeInfo.UserName;
                                }
                                break;
                        }

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
