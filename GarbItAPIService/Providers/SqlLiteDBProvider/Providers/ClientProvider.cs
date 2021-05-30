using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using SQLiteDBProvider.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBProvider.Providers
{
    public class ClientProvider : IClientProvider
    {
        private IDataService _dataService;
        private DBSettings _settings;
        private ICountProvider _countProvider;

        public ClientProvider(IDataService dataService, IOptions<DBSettings> options, ICountProvider countProvider)
        {
            _dataService = dataService;
            _settings = options.Value;
            _countProvider = countProvider;
        }

        public async Task<SearchClientsResponse> SearchClientAsync(List<SearchRequest> searchRequests, int limit = 200, string paginationToken = "")
        {
            var response = await _dataService.SearchData<ClientInfo>(_settings.TableNames.ClientTable, searchRequests, limit, paginationToken);
            if (response.Item1 != null)
            {
                foreach (var item in response.Item1)
                {
                    item.DateOfBirth = item?.DateOfBirth?.ConvertDate();
                    item.CreatedDateTime = item?.CreatedDateTime?.ConvertDate();
                    item.UpdatedDateTime = item?.UpdatedDateTime?.ConvertDate();
                }
            }
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
            var clientInfos = await _dataService.GetData<ClientInfo>(_settings.TableNames.ClientTable, "QRCodeId", qrCodeId);

            var response = clientInfos?.FirstOrDefault();

            if (response != null)
            {
                response.DateOfBirth = response?.DateOfBirth?.ConvertDate();
                response.CreatedDateTime = response?.CreatedDateTime?.ConvertDate();
                response.UpdatedDateTime = response?.UpdatedDateTime?.ConvertDate();
            }
            return response;
        }

        public async Task<AddClientResponse> RegisterClientAsync(ClientInfo clientInfo)
        {
            if (await _dataService.SaveDataSql(_settings.TableNames.ClientTable, clientInfo.ToInsertSqlCmdParams()))
            {
                string counterId = String.Format("{0}-Clients", AmbientContext.Current.UserInfo.Municipality);
                await _countProvider.IncrementCountAsync(counterId, false);

                return new AddClientResponse()
                {
                    Name = clientInfo.Name,
                    QRCodeId = clientInfo.QRCodeId
                };
            }

            return new AddClientResponse();
        }

        public async Task<AddClientResponse> UpdateClientAsync(ClientInfo updateInfo)
        {
            if (await _dataService.UpdateDataSql(_settings.TableNames.ClientTable, updateInfo.Id, updateInfo.ToUpdateSqlCmdParams()))
            {
                return new AddClientResponse()
                {
                    Id = updateInfo.Id,
                    Name = updateInfo.Name,
                    QRCodeId = updateInfo.QRCodeId
                };
            }

            return new AddClientResponse();
        }

        
    }
}
