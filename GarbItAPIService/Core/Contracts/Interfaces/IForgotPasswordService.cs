using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IForgotPasswordService
    {
        Task<List<SecretQuestion>> GetSecretQuestionsAsync();
        Task<AddSecretQuestionResponse> AddSecretQuestionAsync(SecretQuestionAddRequest req);
        Task<SuccessResponse> ForgotPasswordChangeRequestAsync(ForgotPasswordChangeRequest req);
    }
}
