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
        private IPasswordProvider _passwordProvider;
        private AWSDynamoDBSettings _settings;

        public AdminProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
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

            if(await _dataService.SaveData(req, _settings.TableNames.AdminTable))
            {
                var passwordEntry = GetPasswordEntry(req);
                if(await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return true;
                }
            }

            return false;
        }

        

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            return await _dataService.GetDataById<AdminInfo>(id, _settings.TableNames.AdminTable);
        }

        public async Task<bool> RemoveAdminInfoByIdAsync(string id)
        {
            var admin = await GetAdminInfoAsync(id);

            if(admin != null)
            {
                if (await _dataService.RemoveDataByIdAsync<AdminInfo>(id, _settings.TableNames.AdminTable))
                {
                    await _passwordProvider.RemoveUserFromPasswordRegistry(admin.UserName);
                }

                return true;
            }

            return false;
        }

        private static PasswordInfo GetPasswordEntry(Model.AdminInfo req)
        {
            return new PasswordInfo()
            {
                Id = req.AdminId,
                UserName = req.UserName,
                Password = req.Password,
                Role = Role.Admin
            };
        }

        
    }
}
