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
    public class SuperAdminProvider : ISuperAdminProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private DBSettings _settings;

        public SuperAdminProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<DBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _settings = options.Value;
        }

        public async Task<SuperAdminInfo> GetSuperAdminInfoAsync(string id)
        {
            return await _dataService.GetDataById<SuperAdminInfo>(id, _settings.TableNames.SuperAdminTable);
        }

        public async Task<SuperAdminInfo> GetSuperAdminInfoByUserNameAsync(string userName)
        {
            return await _dataService.GetDataByUserName<SuperAdminInfo>(userName, _settings.TableNames.SuperAdminTable);
        }

        public async Task<AddUserResponse> AddSuperAdmin(SuperAdminInfo superAdminInfo)
        {
            if (await _dataService.SaveDataSql(_settings.TableNames.SuperAdminTable, superAdminInfo.ToInsertSqlCmdParams()))
            {
                var passwordEntry = GetPasswordEntry(superAdminInfo);
                if (await _passwordProvider.AddUserToRegistry(passwordEntry))
                {
                    return new AddUserResponse()
                    {
                        Name = superAdminInfo.Name
                    };
                }
            }

            return new AddUserResponse();
        }

        public async Task<AddUserResponse> UpdateSuperAdminAsync(SuperAdminInfo superAdminInfo)
        {
            if (await _dataService.UpdateDataSql(_settings.TableNames.SuperAdminTable, superAdminInfo.Id, superAdminInfo.ToUpdateSqlCmdParams()))
            {
                return new AddUserResponse()
                {
                    Id = superAdminInfo.Id,
                    Name = superAdminInfo.Name
                };
            }

            return new AddUserResponse();
        }

        private static PasswordInfo GetPasswordEntry(SuperAdminInfo req)
        {
            return new PasswordInfo()
            {
                Id = req.Id,
                UserName = req.UserName,
                Password = req.Password,
                Role = Role.SuperAdmin,
                Name = req.Name
            };
        }

        public async Task<SuccessResponse> UpdateSuperAdminPasswordAsync(UpdatePasswordRequest req)
        {
            var userInfo = await GetSuperAdminInfoAsync(req.Id);
            userInfo.Password = req.NewPassword;

            userInfo.UpdatedById = AmbientContext.Current.UserInfo.Name;
            userInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            userInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            var passwordInfo = await _passwordProvider.GetUserPassword(userInfo.UserName);
            passwordInfo.Password = req.NewPassword;

            await UpdateSuperAdminAsync(userInfo);
            var response = await _passwordProvider.UpdatePasswordAsync(passwordInfo);
            return new SuccessResponse() { Success = true };
        }

    }
}
