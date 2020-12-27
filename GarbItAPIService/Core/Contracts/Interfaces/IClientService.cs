using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IClientService
    {
        Task<AddClientResponse> RegisterClientAsync(ClientInfo clientInfo);
        Task<ClientInfo> GetClientInfoAsync(string qrCodeId);
    }
}
