using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class AdminProvider : IAdminProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;

        public AdminProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }

        public async Task<List<AdminInfo>> GetAdmins()
        {
            var response = await _dataService.GetData<AdminInfo>(_settings.TableNames.AdminTable);
            return response;

        }

        public async Task<bool> AddAdmin(AdminInfo adminInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.AdminTable);

            var req = adminInfo.ToDBModel(nextId);

            return await _dataService.SaveData(req, _settings.TableNames.AdminTable);
        }
    }
}
