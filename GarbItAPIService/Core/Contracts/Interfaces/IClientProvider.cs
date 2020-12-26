using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IClientProvider
    {
        Task<bool> RegisterClientAsync(ClientInfo clientInfo);
        Task<ClientInfo> GetClientInfoAsync(string qrCodeId);
    }
}
