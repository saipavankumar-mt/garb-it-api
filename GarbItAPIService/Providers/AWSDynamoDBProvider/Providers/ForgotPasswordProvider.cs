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
        private DBSettings _settings;

        public ForgotPasswordProvider(IDataService dataService, IOptions<DBSettings> options)
        {
            _dataService = dataService;
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
   
    }
}
