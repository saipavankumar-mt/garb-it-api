using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Threading.Tasks;

namespace SessionService
{
    public class SessionService : ISessionService
    {
        private ISessionProvider _sessionProvider;
        
        public SessionService(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public async Task<SessionResponse> CreateSessionAsync(LoginRequest loginRequest)
        {
            return await _sessionProvider.CreateSessionAsync(loginRequest);
        }

        public async Task<SessionInfo> GetSessionInfoAsync(string sessionKey)
        {
            return await _sessionProvider.GetSessionInfoAsync(sessionKey);
        }
    }
}
