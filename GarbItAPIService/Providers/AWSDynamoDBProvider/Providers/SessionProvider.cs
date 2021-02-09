using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class SessionProvider : ISessionProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private IAdminProvider _adminProvider;
        private IEmployeeProvider _employeeProvider;
        private ISuperAdminProvider _superAdminProvider;
        private AWSDynamoDBSettings _settings;

        public SessionProvider(IDataService dataService, IPasswordProvider passwordProvider, IAdminProvider adminProvider, IEmployeeProvider employeeProvider, ISuperAdminProvider superAdminProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _adminProvider = adminProvider;
            _superAdminProvider = superAdminProvider;
            _employeeProvider = employeeProvider;
            _settings = options.Value;
        }

        public async Task<SessionInfo> GetSessionInfoAsync(string sessionKey)
        {
            var sessionInfo = await _dataService.GetDataById<Model.SessionInfo>(sessionKey, _settings.TableNames.SessionTable);

            await SaveUserInfoToContext(sessionInfo);

            return sessionInfo?.ToEntityModel();
        }

        public async Task<SessionResponse> CreateSessionAsync(LoginRequest loginRequest)
        {
            var user = await _passwordProvider.GetUserPassword(loginRequest.UserName);
            if (user != null)
            {
                var sessionResponse = await SaveSessionAsync(loginRequest, user);
                return sessionResponse;
            }

            return new SessionResponse();
        }

        private async Task<SessionResponse> SaveSessionAsync(LoginRequest loginRequest, PasswordInfo user)
        {
            if (loginRequest.Password.Equals(user.Password) && loginRequest.Role == user.Role)
            {
                var sessionKey = Guid.NewGuid().ToString();

                var req = new SessionInfo()
                {
                    UserName = loginRequest.UserName,
                    UserId = user.Id,
                    Role = loginRequest.Role,
                    UserFullName = user.Name
                };

                var dbReq = req.ToDBModel(sessionKey);

                if (await _dataService.SaveData(dbReq, _settings.TableNames.SessionTable))
                {
                    return new SessionResponse()
                    {
                        SessionKey = sessionKey,
                        Id = user.Id,
                        Name = user.Name,
                        Role = user.Role
                    };
                }
            }

            return null;
        }


        private async Task SaveUserInfoToContext(Model.SessionInfo sessionInfo)
        {
            if (sessionInfo != null)
            {
                switch (sessionInfo.Role)
                {
                    case "SuperAdmin":
                        {
                            var userInfo = await _superAdminProvider.GetSuperAdminInfoAsync(sessionInfo.UserId);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    case "Admin":
                        {
                            var userInfo = await _adminProvider.GetAdminInfoAsync(sessionInfo.UserId);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    case "Employee":
                        {
                            var userInfo = await _employeeProvider.GetEmployeeInfoAsync(sessionInfo.UserId);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}