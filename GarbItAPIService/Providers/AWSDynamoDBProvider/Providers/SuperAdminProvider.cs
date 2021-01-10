using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class SuperAdminProvider : ISuperAdminProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private AWSDynamoDBSettings _settings;

        public SuperAdminProvider(IDataService dataService, IPasswordProvider passwordProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _settings = options.Value;
        }

        public async Task<SuperAdminInfo> GetSuperAdminInfoAsync(string id)
        {
            return await _dataService.GetDataById<SuperAdminInfo>(id, _settings.TableNames.SuperAdminTable);
        }

        public async Task<AddUserResponse> AddSuperAdmin(SuperAdminInfo superAdminInfo)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.SuperAdminTable, _settings.UserIdPrefix.SuperAdmin, _settings.NextIdGeneratorValue.SuperAdmin);

            var req = superAdminInfo.ToDBModel(nextId);

            if (await _dataService.SaveData(req, _settings.TableNames.SuperAdminTable))
            {
                var passwordEntry = GetPasswordEntry(req);
                if (await _passwordProvider.AddUserToRegistry(passwordEntry))
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

        public async Task<AddUserResponse> UpdateSuperAdminAsync(SuperAdminInfo superAdminInfo)
        {
            var req = superAdminInfo.ToDBModel();

            if (await _dataService.UpdateData(req, _settings.TableNames.SuperAdminTable))
            {
                return new AddUserResponse()
                {
                    Id = req.Id,
                    Name = req.Name
                };
            }

            return new AddUserResponse();
        }

        private static PasswordInfo GetPasswordEntry(Model.SuperAdminInfo req)
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
