using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Threading.Tasks;

namespace ClientService
{
    public class ClientService : IClientService
    {
        private IClientProvider _clientProvider;

        public ClientService(IClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public async Task<ClientInfo> GetClientInfoAsync(string qrCodeId)
        {
            return await _clientProvider.GetClientInfoAsync(qrCodeId);
        }

        public async Task<bool> RegisterClientAsync(ClientInfo clientInfo)
        {
            clientInfo.CreatedDate = DateTime.Now.ToString();

            return await _clientProvider.RegisterClientAsync(clientInfo);
        }
    }
}
