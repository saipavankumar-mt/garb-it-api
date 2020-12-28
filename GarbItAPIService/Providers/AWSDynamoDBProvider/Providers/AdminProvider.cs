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

        public async Task<List<AdminInfo>> GetAdmins(string reportsToId = "")
        {
            var response = new List<AdminInfo>();

            if (string.IsNullOrEmpty(reportsToId))
            {
                response = await _dataService.GetData<AdminInfo>(_settings.TableNames.AdminTable);
            }
            else
            {
                response = await _dataService.GetData<AdminInfo>(_settings.TableNames.AdminTable, "ReportsToId", reportsToId);
            }

            return response;
        }

        public async Task<AddUserResponse> AddAdmin(AdminInfo adminInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.AdminTable, _settings.NextIdGeneratorValue.Admin);

            var req = adminInfo.ToDBModel(nextId);

            if(await _dataService.SaveData(req, _settings.TableNames.AdminTable))
            {
                var passwordEntry = GetPasswordEntry(req);
                if(await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return new AddUserResponse()
                    {
                        Id = nextId,
                        Name = req.Name
                    };
                }
            }

            return new AddUserResponse();
        }

        public async Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo)
        {
            var req = adminInfo.ToDBModel();

            if (await _dataService.UpdateData(req, _settings.TableNames.AdminTable))
            {
                return new AddUserResponse()
                {
                    Id = req.Id,
                    Name = req.Name
                };
            }

            return new AddUserResponse();
        }

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            return await _dataService.GetDataById<AdminInfo>(id, _settings.TableNames.AdminTable);
        }

        public async Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id)
        {
            var admin = await GetAdminInfoAsync(id);

            if(admin != null)
            {
                if (await _dataService.RemoveDataByIdAsync<AdminInfo>(id, _settings.TableNames.AdminTable))
                {
                    await _passwordProvider.RemoveUserFromPasswordRegistry(admin.UserName);
                }

                return new RemoveUserResponse()
                {
                    Id = admin.Id,
                    Name = admin.Name
                };
            }

            return new RemoveUserResponse();
        }

        private static PasswordInfo GetPasswordEntry(Model.AdminInfo req)
        {
            return new PasswordInfo()
            {
                Id = req.Id,
                UserName = req.UserName,
                Password = req.Password,
                Role = Role.Admin,
                Name = req.Name
            };
        }

        
    }
}
