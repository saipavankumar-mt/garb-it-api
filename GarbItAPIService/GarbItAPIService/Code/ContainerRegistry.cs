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

            services.AddTransient<IDataService, AWSDynamoDBProvider.Services.AWSDataService>();

            services.AddTransient<ISuperAdminService, SuperAdminService.SuperAdminService>();
            services.AddTransient<ISuperAdminProvider, AWSDynamoDBProvider.Providers.SuperAdminProvider>();

            services.AddTransient<IAdminService, AdminService.AdminService>();
            services.AddTransient<IAdminProvider, AWSDynamoDBProvider.Providers.AdminProvider>();

            services.AddTransient<IEmployeeService, EmployeeService.EmployeeService>();
            services.AddTransient<IEmployeeProvider, AWSDynamoDBProvider.Providers.EmployeeProvider>();

            services.AddTransient<IPasswordProvider, AWSDynamoDBProvider.Providers.PasswordProvider>();
            
            services.AddTransient<ISessionProvider, AWSDynamoDBProvider.Providers.SessionProvider>();
            services.AddTransient<ISessionService, SessionService.SessionService>();

            services.AddTransient<IRecordEntryService, RecordEntryService.RecordEntryService>();
            services.AddTransient<IRecordEntryProvider, AWSDynamoDBProvider.Providers.RecordEntryProvider>();

            services.AddTransient<IClientService, ClientService.ClientService>();
            services.AddTransient<IClientProvider, AWSDynamoDBProvider.Providers.ClientProvider>();

            services.AddTransient<IForgotPasswordService, ForgotPasswordService.ForgotPasswordService>();
            services.AddTransient<IForgotPasswordProvider, AWSDynamoDBProvider.Providers.ForgotPasswordProvider>();

            services.AddTransient<ICountProvider, AWSDynamoDBProvider.Providers.CountProvider>();
        }
    }
}
