using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class AdminProvider : IAdminProvider
    {
        private IDataService _dataService;

        public AdminProvider(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<AdminInfo>> GetAdmins()
        {
            var response = await _dataService.GetData<AdminInfo>("AdminInfo");
            return response;

        }

        public async Task<bool> AddAdmin(AdminInfo adminInfo)
        {
            var nextId = await _dataService.GetNextId("AdminInfo");

            var req = new Model.AdminInfo()
            {
                AdminId = nextId,
                AdminName = adminInfo.AdminName
            };

            return await _dataService.SaveData(req, "AdminInfo");
        }
    }
}
