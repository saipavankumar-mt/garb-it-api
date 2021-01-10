using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IForgotPasswordProvider
    {
        Task<List<SecretQuestion>> GetSecretQuestionsAsync();
        Task<AddSecretQuestionResponse> AddSecretQuestionAsync(SecretQuestionAddRequest req);
        Task<SecretQuestion> GetSecretQuestionByIdAsync(string id);
        Task<SuccessResponse> ForgotPasswordChangeRequestAsync(ForgotPasswordChangeRequest req);
    }
}
