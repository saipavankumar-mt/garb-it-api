using Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbItAPIService.Code
{
    public static class ContainerRegistry
    {
        public static void Init(IServiceCollection services)
        {
            services.AddTransient<IDataService, AWSDynamoDBProvider.Services.AWSDataService>();

            services.AddTransient<IAdminService, AdminService.AdminService>();
            services.AddTransient<IAdminProvider, AWSDynamoDBProvider.Providers.AdminProvider>();

            services.AddTransient<IEmployeeService, EmployeeService.EmployeeService>();
            services.AddTransient<IEmployeeProvider, AWSDynamoDBProvider.Providers.EmployeeProvider>();

                        
        }
    }
}
