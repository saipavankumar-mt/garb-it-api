using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class ClientProvider : IClientProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;
        private ICountProvider _countProvider;

        public ClientProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options, ICountProvider countProvider)
        {
            _dataService = dataService;
            _settings = options.Value;
            _countProvider = countProvider;
        }

        public async Task<SearchClientsResponse> SearchClientAsync(List<SearchRequest> searchRequests, int limit = 200, string paginationToken = "")
        {
            var response = await _dataService.SearchData<ClientInfo>(_settings.TableNames.ClientTable, searchRequests, limit, paginationToken);

            return new SearchClientsResponse()
            {
                ClientInfos = response.Item1,
                PaginationToken = response.Item2
            }; 
        }

        public async Task<int> SearchClientCountAsync(SearchRequest searchRequest=null)
        {
            string counterId = String.Format("{0}-Clients", AmbientContext.Current.UserInfo.Municipality);
            var countInfo = await _countProvider.GetCountInfoAsync(counterId);
            if(countInfo!=null)
            {
                return countInfo.Count;
            }

            return 0;
        }

        public async Task<ClientInfo> GetClientInfoAsync(string qrCodeId)
        {
            return await _dataService.GetDataById<ClientInfo>(qrCodeId, _settings.TableNames.ClientTable);
        }

        public async Task<AddClientResponse> RegisterClientAsync(ClientInfo clientInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.ClientTable, _settings.UserIdPrefix.Client, _settings.NextIdGeneratorValue.Client, "D8");

            var req = clientInfo.ToDBModel(nextId);

            if (await _dataService.SaveData(req, _settings.TableNames.ClientTable))
            {
                string counterId = String.Format("{0}-Clients", AmbientContext.Current.UserInfo.Municipality);
                await _countProvider.IncrementCountAsync(counterId);

                return new AddClientResponse()
                {
                    Id = req.Id,
                    Name = req.Name,
                    QRCodeId = req.QRCodeId
                };
            }

            return new AddClientResponse();
        }

        public async Task<AddClientResponse> UpdateClientAsync(ClientInfo updateInfo)
        {
            var req = updateInfo.ToDBModel();
            if (await _dataService.UpdateData(req, _settings.TableNames.ClientTable))
            {
                return new AddClientResponse()
                {
                    Id = req.Id,
                    Name = req.Name,
                    QRCodeId = req.QRCodeId
                };
            }

            return new AddClientResponse();
        }

        
    }
}
