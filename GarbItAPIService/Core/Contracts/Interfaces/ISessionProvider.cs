﻿using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ISessionProvider
    {
        Task<SessionResponse> CreateSessionAsync(LoginRequest loginRequest);
        Task<SessionInfo> GetSessionInfoAsync(string sessionKey);
    }
}
