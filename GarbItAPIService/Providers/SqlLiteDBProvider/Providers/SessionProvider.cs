using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using SQLiteDBProvider.Translator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBProvider.Providers
{
    public class SessionProvider : ISessionProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private IAdminProvider _adminProvider;
        private IEmployeeProvider _employeeProvider;
        private ISuperAdminProvider _superAdminProvider;
        private DBSettings _settings;

        public SessionProvider(IDataService dataService, IPasswordProvider passwordProvider, IAdminProvider adminProvider, IEmployeeProvider employeeProvider, ISuperAdminProvider superAdminProvider, IOptions<DBSettings> options)
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
            var sessionInfo = await _dataService.GetDataById<SessionInfo>(sessionKey, _settings.TableNames.SessionTable);

            await SaveUserInfoToContext(sessionInfo);

            return sessionInfo;
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
                var userInfo = await GetUserInfo(loginRequest);

                if(userInfo != null)
                {
                    var sessionKey = Guid.NewGuid().ToString();

                    var req = new SessionInfo()
                    {
                        Id = sessionKey,
                        UserName = userInfo.UserName,
                        UserId = userInfo.Id,
                        Role = userInfo.Role,
                        UserFullName = userInfo.Name,
                        SessionCreatedOn = DateTime.Now.ToString(),
                        Municipality = userInfo.Municipality
                    };

                    if (await _dataService.SaveDataSql(_settings.TableNames.SessionTable, req.ToInsertSqlCmdParams()))
                    {
                        return new SessionResponse()
                        {
                            SessionKey = sessionKey,
                            Id = userInfo.Id,
                            Name = userInfo.Name,
                            Role = userInfo.Role,
                            Municipality = userInfo.Municipality
                        };
                    }
                }
            }

            return null;
        }


        private async Task SaveUserInfoToContext(SessionInfo sessionInfo)
        {
            if (sessionInfo != null)
            {
                switch (sessionInfo.Role.ToString())
                {
                    case "SuperAdmin":
                        {
                            var userInfo = await _superAdminProvider.GetSuperAdminInfoByUserNameAsync(sessionInfo.UserName);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    case "Admin":
                        {
                            var userInfo = await _adminProvider.GetAdminInfoByUserNameAsync(sessionInfo.UserName);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    case "Employee":
                        {
                            var userInfo = await _employeeProvider.GetEmployeeInfoByUserNameAsync(sessionInfo.UserName);
                            AmbientContext.Current.UserInfo = userInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task<UserInfo> GetUserInfo(LoginRequest request)
        {
            if (request != null)
            {
                switch (request.Role.ToString())
                {
                    case "SuperAdmin":
                        {
                            var userInfo = await _superAdminProvider.GetSuperAdminInfoByUserNameAsync(request.UserName);
                            return userInfo;
                        }
                    case "Admin":
                        {
                            var userInfo = await _adminProvider.GetAdminInfoByUserNameAsync(request.UserName);
                            return userInfo;
                        }
                    case "Employee":
                        {
                            var userInfo = await _employeeProvider.GetEmployeeInfoByUserNameAsync(request.UserName);
                            return userInfo;
                        }
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}