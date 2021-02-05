using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
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

        public async Task<CountResponse> GetClientsCountAsync()
        {
            var role = AmbientContext.Current.UserInfo.Role;
            int totalCount = 0;

            if (role.Equals(Role.SuperAdmin))
            {
                totalCount = await _clientProvider.SearchClientCountAsync(new SearchRequest()
                {
                    SearchByKey = "Location",
                    SearchByValue = AmbientContext.Current.UserInfo.Location
                });
            }

            if (role.Equals(Role.Admin))
            {
                totalCount = await _clientProvider.SearchClientCountAsync(new SearchRequest()
                {
                    SearchByKey = "Municipality",
                    SearchByValue = AmbientContext.Current.UserInfo.Municipality
                });
            }

            return new CountResponse()
            {
                Count = totalCount
            };
        }

        public async Task<AddClientResponse> RegisterClientAsync(ClientAddRequest clientAddRequest)
        {
            var clientInfo = clientAddRequest.ToCoreModel();
            return await _clientProvider.RegisterClientAsync(clientInfo);
        }

        public async Task<SearchClientsResponse> SearchClientAsync(List<SearchRequest> searchRequests, int limit = 200, string paginationToken = "")
        {
            return await _clientProvider.SearchClientAsync(searchRequests, limit, paginationToken);
        }

        public async Task<AddClientResponse> UpdateClientAsync(ClientInfo clientInfo)
        {
            clientInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            clientInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            clientInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;

            return await _clientProvider.UpdateClientAsync(clientInfo);
        }
    }
}
