﻿using Contracts.Interfaces;
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

        public ClientProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }

        public async Task<ClientInfo> GetClientInfoAsync(string qrCodeId)
        {
            return await _dataService.GetDataById<ClientInfo>(qrCodeId, _settings.TableNames.ClientTable);
        }

        public async Task<AddClientResponse> RegisterClientAsync(ClientInfo clientInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.ClientTable, _settings.NextIdGeneratorValue.Client);

            var req = clientInfo.ToDBModel(nextId);

            if (await _dataService.SaveData(req, _settings.TableNames.ClientTable))
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