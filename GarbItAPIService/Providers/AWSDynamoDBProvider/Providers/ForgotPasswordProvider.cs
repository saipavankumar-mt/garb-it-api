using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSDynamoDBProvider.Providers
{
    public class ForgotPasswordProvider : IForgotPasswordProvider
    {
        private IDataService _dataService;
        private IAdminProvider _adminProvider;
        private AWSDynamoDBSettings _settings;

        public ForgotPasswordProvider(IDataService dataService, IAdminProvider adminProvider, IOptions<AWSDynamoDBSettings> options)
        {
            _dataService = dataService;
            _adminProvider = adminProvider;
            _settings = options.Value;
        }

        public async Task<List<SecretQuestion>> GetSecretQuestionsAsync()
        {
            return await _dataService.GetData<SecretQuestion>(_settings.TableNames.SecretQuestionsTable);
        }

        public async Task<SecretQuestion> GetSecretQuestionByIdAsync(string id)
        {
            return await _dataService.GetDataById<SecretQuestion>(id, _settings.TableNames.SecretQuestionsTable);
        }


        public async Task<AddSecretQuestionResponse> AddSecretQuestionAsync(SecretQuestionAddRequest req)
        {
            var nextId = await _dataService.GetNextId(_settings.TableNames.SecretQuestionsTable, _settings.UserIdPrefix.SecretQuestion, _settings.NextIdGeneratorValue.SecretQuestion);

            var dbReq = new Model.SecretQuestion()
            {
                Id = nextId,
                Question = req.Question
            };

            if (await _dataService.SaveData(req, _settings.TableNames.SecretQuestionsTable))
            {
                return new AddSecretQuestionResponse()
                {
                    Id = nextId
                };
            }

            return new AddSecretQuestionResponse();
        }

        public async Task<SuccessResponse> ForgotPasswordChangeRequestAsync(ForgotPasswordChangeRequest req)
        {
            var userinfo = await _adminProvider.GetAdminInfoAsync(req.Id);
            if (userinfo != null)
            {
                var qIds = userinfo.SecretQuestions;
                var ans = userinfo.SecretAnswers;

                if(AreEqual(ans, req.Answers))
                {
                    var updatePasswordRequest = new UpdatePasswordRequest()
                    {
                        Id = req.Id,
                        NewPassword = req.NewPassword
                    };

                    await _adminProvider.UpdateAdminPasswordAsync(updatePasswordRequest);
                    return new SuccessResponse() { Success = true };
                }
                else
                {
                    throw new Exception("Security answers are not matching");
                }
                
            }
            else
            {
                throw new Exception("User id not Valid");
            }
        }

        private bool AreEqual(List<string> list1, List<string> list2)
        {
            var firstNotSecond = list1.Except(list2, StringComparer.OrdinalIgnoreCase).ToList();
            var secondNotFirst = list2.Except(list1, StringComparer.OrdinalIgnoreCase).ToList();

            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }
    }
}
