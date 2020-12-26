using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ISessionService
    {
        Task<string> CreateSessionAsync(LoginRequest loginRequest);

        Task<SessionInfo> GetSessionInfoAsync(string sessionKey);
    }
}
