using AWSDynamoDBProvider.Model;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class PasswordProvider : IPasswordProvider
    {
        private IDataService _dataService;
        private AWSDynamoDBSettings _settings;

        public PasswordProvider(IDataService dataService, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }

        public async Task<bool> AddUserToRegistry(PasswordInfo passwordInfo)
        {
            var req = passwordInfo.ToDBModel();
            return await _dataService.SaveData(req, _settings.TableNames.PasswordRegistryTable);

        }

        public async Task<PasswordInfo> GetUserPassword(string userName)
        {
            var response = await _dataService.GetDataByUserName<PasswordRegistry>(userName, _settings.TableNames.PasswordRegistryTable);

            return response.ToEntityModel();
        }

        public async Task<bool> RemoveUserFromPasswordRegistry(string userName)
        {
            return await _dataService.RemoveDataByIdAsync<PasswordRegistry>(userName, _settings.TableNames.PasswordRegistryTable);
        }
    }
}
