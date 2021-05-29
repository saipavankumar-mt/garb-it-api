using Contracts;
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
    public class AdminProvider : IAdminProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private DBSettings _settings;

        public AdminProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<DBSettings> options)
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
            if (response != null)
            {
                foreach (var item in response)
                {
                    item.DateOfBirth = item?.DateOfBirth?.ConvertDate();
                    item.CreatedDateTime = item?.CreatedDateTime?.ConvertDate();
                    item.UpdatedDateTime = item?.UpdatedDateTime?.ConvertDate();
                } 
            }
            return response;
        }

        public async Task<int> GetAdminsCount(string reportsToId = "")
        {
            if (string.IsNullOrEmpty(reportsToId))
            {
                return await _dataService.GetDataCount(_settings.TableNames.AdminTable);
            }
            else
            {
                return await _dataService.GetDataCount(_settings.TableNames.AdminTable, "ReportsToId", reportsToId);
            }
        }

        public async Task<AddUserResponse> AddAdmin(AdminInfo adminInfo)
        {
            if(await _dataService.SaveDataSql(_settings.TableNames.AdminTable, adminInfo.ToInsertSqlCmdParams()))
            {
                var passwordEntry = GetPasswordEntry(adminInfo);
                if(await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return new AddUserResponse()
                    {
                        Name = adminInfo.Name
                    };
                }
            }

            return new AddUserResponse();
        }

        public async Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo)
        {
            if (await _dataService.UpdateDataSql(_settings.TableNames.AdminTable, adminInfo.Id, adminInfo.ToUpdateSqlCmdParams()))
            {
                return new AddUserResponse()
                {
                    Id = adminInfo.Id,
                    Name = adminInfo.Name
                };
            }

            return new AddUserResponse();
        }

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            var response = await _dataService.GetDataById<AdminInfo>(id, _settings.TableNames.AdminTable);

            if (response != null)
            {
                response.DateOfBirth = response?.DateOfBirth?.ConvertDate();
                response.CreatedDateTime = response?.CreatedDateTime?.ConvertDate();
                response.UpdatedDateTime = response?.UpdatedDateTime?.ConvertDate();
            }
            return response;
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

        private static PasswordInfo GetPasswordEntry(AdminInfo req)
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

        public async Task<SuccessResponse> UpdateAdminPasswordAsync(UpdatePasswordRequest req)
        {
            var adminInfo = await GetAdminInfoAsync(req.Id);
            adminInfo.Password = req.NewPassword;

            adminInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            adminInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            adminInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var passwordInfo = await _passwordProvider.GetUserPassword(adminInfo.UserName);
            passwordInfo.Password = req.NewPassword;

            await UpdateAdminAsync(adminInfo);
            var response = await _passwordProvider.UpdatePasswordAsync(passwordInfo);
            return new SuccessResponse() { Success = true };
        }

        public async Task<AdminInfo> GetAdminInfoByUserNameAsync(string userName)
        {
            var response = await _dataService.GetDataByUserName<AdminInfo>(userName, _settings.TableNames.AdminTable);
            if (response != null)
            {
                response.DateOfBirth = response?.DateOfBirth?.ConvertDate();
                response.CreatedDateTime = response?.CreatedDateTime?.ConvertDate();
                response.UpdatedDateTime = response?.UpdatedDateTime?.ConvertDate();
            }
            return response;
        }
    }
}
