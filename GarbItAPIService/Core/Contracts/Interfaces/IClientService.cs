using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IClientService
    {
        Task<AddClientResponse> RegisterClientAsync(ClientAddRequest clientInfo);
        Task<ClientInfo> GetClientInfoAsync(string qrCodeId);
        Task<AddClientResponse> UpdateClientAsync(ClientInfo updateInfo);
    }
}
