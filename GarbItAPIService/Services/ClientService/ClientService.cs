using Contracts;
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

        public async Task<AddClientResponse> RegisterClientAsync(ClientAddRequest clientAddRequest)
        {
            var clientInfo = clientAddRequest.ToCoreModel();
            return await _clientProvider.RegisterClientAsync(clientInfo);
        }

        public async Task<AddClientResponse> UpdateClientAsync(ClientInfo clientInfo)
        {
            clientInfo.UpdatedDateTime = DateTime.Now.ToString();
            clientInfo.UpdatedById = AmbientContext.Current.UserId;
            clientInfo.UpdatedByName = AmbientContext.Current.UserName;

            return await _clientProvider.UpdateClientAsync(clientInfo);
        }
    }
}
