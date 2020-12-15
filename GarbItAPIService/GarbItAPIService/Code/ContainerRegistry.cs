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
            services.AddSingleton<IDataService, DataService.DataService>();
            services.AddSingleton<IAdminService, AdminService.AdminService>();
            services.AddSingleton<ISuperAdminService, SuperAdminService.SuperAdminService>();
            services.AddSingleton<IEmployeeService, EmployeeService.EmployeeService>();
        }
    }
}
