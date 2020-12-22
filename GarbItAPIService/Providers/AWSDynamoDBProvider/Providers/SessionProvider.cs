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

        public async Task<string> CreateSessionAsync(LoginRequest loginRequest)
        {
            if(await IsValidUser(loginRequest))
            {
                var sessionKey = Guid.NewGuid().ToString();

                var req = new SessionInfo()
                {
                    UserName = loginRequest.UserName,
                    Role = loginRequest.Role
                };

                var dbReq = req.ToDBModel(sessionKey);

                if(await _dataService.SaveData(dbReq, _settings.TableNames.SessionTable))
                {
                    return sessionKey;
                }
            }

            return string.Empty;


        }

        private async Task<bool> IsValidUser(LoginRequest loginRequest)
        {
            var user = await _passwordProvider.GetUserPassword(loginRequest.UserName);
            if(user!=null)
            {
                if(loginRequest.Password.Equals(user.Password) && loginRequest.Role == user.Role)
                {
                    return true;
                }
            }

            return false;
        }
    }
}