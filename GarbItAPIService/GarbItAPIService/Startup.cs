using Autofac;
using AWSDynamoDBProvider;
using Contracts;
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

        private string _corsPolicyName = "All";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
               options.AddPolicy(_corsPolicyName,
                   p => p   
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
               ));


            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<DBSettings>(Configuration.GetSection("DBSettings"));

            services.AddSwaggerDocument();


            
            ContainerRegistry.Init(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_corsPolicyName);

            app.UseRouting();

            app.UseAuthorization();

            app.RegisterMiddlewares();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
