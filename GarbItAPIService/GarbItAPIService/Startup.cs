using AWSDynamoDBProvider;
using Contracts.Models;
using GarbItAPIService.Code;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GarbItAPIService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<AWSDynamoDBSettings>(Configuration.GetSection("AWSDynamoDBSettings"));

            // Aws DynamoDb service setup
            services.AwsDynamoDbServiceSetup(Configuration);

            ContainerRegistry.Init(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddCors(IServiceCollection services)
        {
            var allowOrigins = Configuration.GetSection("AllowedOrigins").Value.Split(',');
            services.AddCors(options =>
               options.AddPolicy("All",
                   p => p
                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                   .WithOrigins(allowOrigins)
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .AllowAnyHeader()
               ));
        }
    }
}
