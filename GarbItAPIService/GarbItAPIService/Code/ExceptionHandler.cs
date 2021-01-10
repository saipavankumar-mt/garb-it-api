using Contracts.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbItAPIService.Code
{
    public class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Error errResponse = new Error() { Message = ex.Message };
                var res = JsonConvert.SerializeObject(new ApiSuccessResponse<Error>(errResponse) { Status = Status.Failure }, new Newtonsoft.Json.Converters.StringEnumConverter());
                context.Response.ContentType = "application/json";
                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(res));
            }
        }
    }
}
