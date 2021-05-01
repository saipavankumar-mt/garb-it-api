using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using SQLiteDBProvider.Translator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDBProvider.Providers
{
    public class PasswordProvider : IPasswordProvider
    {
        private IDataService _dataService;
        private DBSettings _settings;

        public PasswordProvider(IDataService dataService, IOptions<DBSettings> options)
        {
            _dataService = dataService;
            _settings = options.Value;
        }

        public async Task<bool> AddUserToRegistry(PasswordInfo passwordInfo)
        {
            return await _dataService.SaveDataSql(_settings.TableNames.PasswordRegistryTable, passwordInfo.ToInsertSqlCmdParams());
        }

        public async Task<PasswordInfo> GetUserPassword(string userName)
        {
            var response = await _dataService.GetDataByUserName<PasswordInfo>(userName, _settings.TableNames.PasswordRegistryTable);

            return response;
        }

        public async Task<bool> RemoveUserFromPasswordRegistry(string userName)
        {
            return await _dataService.RemoveDataByIdAsync<PasswordInfo>(userName, _settings.TableNames.PasswordRegistryTable);
        }

        public async Task<bool> UpdatePasswordAsync(PasswordInfo passwordInfo)
        {
            return await _dataService.UpdateDataSql(_settings.TableNames.PasswordRegistryTable, passwordInfo.UserName, passwordInfo.ToUpdateSqlCmdParams());
        }
    }
}
