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
    public class AdminProvider : IAdminProvider
    {
        private IDataService _dataService;
        private IPasswordProvider _passwordProvider;
        private IForgotPasswordProvider _forgotPasswordProvider;
        private AWSDynamoDBSettings _settings;

        public AdminProvider(IDataService dataService, IPasswordProvider passwordProvider, IForgotPasswordProvider forgotPasswordProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _passwordProvider = passwordProvider;
            _forgotPasswordProvider = forgotPasswordProvider;
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
            var nextId = await _dataService.GetNextId(_settings.TableNames.AdminTable, _settings.UserIdPrefix.Admin, _settings.NextIdGeneratorValue.Admin);

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

        public async Task<SuccessResponse> UpdateAdminPasswordAsync(UpdatePasswordRequest req)
        {
            var adminInfo = await GetAdminInfoAsync(req.Id);
            adminInfo.Password = req.NewPassword;

            adminInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            adminInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            adminInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            var passwordInfo = await _passwordProvider.GetUserPassword(adminInfo.UserName);
            passwordInfo.Password = req.NewPassword;

            await UpdateAdminAsync(adminInfo);
            var response = await _passwordProvider.UpdatePasswordAsync(passwordInfo);
            return new SuccessResponse() { Success = true };
        }

        public async Task<SuccessResponse> UpdateSecretQuestionsAsync(AddUserSecretQuestionsRequest req)
        {
            var adminInfo = await GetAdminInfoAsync(req.Id);
            adminInfo.SecretQuestions = req.QuestionIds;
            adminInfo.SecretAnswers = req.Answers;

            await UpdateAdminAsync(adminInfo);
            return new SuccessResponse() { Success = true };
        }

        public async Task<List<SecretQuestion>> GetUserSecretQuestionsAsync(string id)
        {
            var response = new List<SecretQuestion>();

            var adminInfo = await GetAdminInfoAsync(id);
            var questionIds = adminInfo.SecretQuestions;

            foreach (var qId in questionIds)
            {
                response.Add(await _forgotPasswordProvider.GetSecretQuestionByIdAsync(qId));
            }

            return response;
        }
    }
}
