using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbItAPIService.Code
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder RegisterMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMiddleware<AmbientContextMiddleware>();            
            return app;
        }
    }
}
