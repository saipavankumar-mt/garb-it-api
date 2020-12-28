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
        private AWSDynamoDBSettings _settings;

        public SessionProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _settings = options.Value;
        }

        public async Task<SessionInfo> GetSessionInfoAsync(string sessionKey)
        {
            var sessionInfo = await _dataService.GetDataById<Model.SessionInfo>(sessionKey, _settings.TableNames.SessionTable);

            return sessionInfo.ToEntityModel();
        }

        public async Task<SessionResponse> CreateSessionAsync(LoginRequest loginRequest)
        {
            var user = await _passwordProvider.GetUserPassword(loginRequest.UserName);
            if (user != null)
            {
                if (loginRequest.Password.Equals(user.Password) && loginRequest.Role == user.Role)
                {
                    var sessionKey = Guid.NewGuid().ToString();

                    var req = new SessionInfo()
                    {
                        UserName = loginRequest.UserName,
                        UserId = user.Id,
                        Role = loginRequest.Role,
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
            }

            return new SessionResponse();
        }
    }
}