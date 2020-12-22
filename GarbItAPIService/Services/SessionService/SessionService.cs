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

        public async Task<string> CreateSessionAsync(LoginRequest loginRequest)
        {
            return await _sessionProvider.CreateSessionAsync(loginRequest);
        }
    }
}
