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
            services.AddTransient<AmbientContextMiddleware, AmbientContextMiddleware>();
            services.AddTransient<ExceptionHandler, ExceptionHandler>();

            services.AddTransient<IDataService, SQLiteDBProvider.Services.SQLiteDataService>();

            services.AddTransient<ISuperAdminService, SuperAdminService.SuperAdminService>();
            services.AddTransient<ISuperAdminProvider, SQLiteDBProvider.Providers.SuperAdminProvider>();

            services.AddTransient<IAdminService, AdminService.AdminService>();
            services.AddTransient<IAdminProvider, SQLiteDBProvider.Providers.AdminProvider>();

            services.AddTransient<IEmployeeService, EmployeeService.EmployeeService>();
            services.AddTransient<IEmployeeProvider, SQLiteDBProvider.Providers.EmployeeProvider>();

            services.AddTransient<IPasswordProvider, SQLiteDBProvider.Providers.PasswordProvider>();
            
            services.AddTransient<ISessionProvider, SQLiteDBProvider.Providers.SessionProvider>();
            services.AddTransient<ISessionService, SessionService.SessionService>();

            services.AddTransient<IRecordEntryService, RecordEntryService.RecordEntryService>();
            services.AddTransient<IRecordEntryProvider, SQLiteDBProvider.Providers.RecordEntryProvider>();

            services.AddTransient<IClientService, ClientService.ClientService>();
            services.AddTransient<IClientProvider, SQLiteDBProvider.Providers.ClientProvider>();

            services.AddTransient<ICountProvider, SQLiteDBProvider.Providers.CountProvider>();
        }
    }
}
