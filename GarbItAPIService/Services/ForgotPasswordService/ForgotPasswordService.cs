using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForgotPasswordService
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private IForgotPasswordProvider _forgotPasswordProvider;

        public ForgotPasswordService(IForgotPasswordProvider forgotPasswordProvider)
        {
            _forgotPasswordProvider = forgotPasswordProvider;
        }

        public async Task<AddSecretQuestionResponse> AddSecretQuestionAsync(SecretQuestionAddRequest req)
        {
            return await _forgotPasswordProvider.AddSecretQuestionAsync(req);
        }

        public async Task<SuccessResponse> ForgotPasswordChangeRequestAsync(ForgotPasswordChangeRequest req)
        {
            return await _forgotPasswordProvider.ForgotPasswordChangeRequestAsync(req);
        }

        public async Task<List<SecretQuestion>> GetSecretQuestionsAsync()
        {
            return await _forgotPasswordProvider.GetSecretQuestionsAsync();
        }
    }
}
