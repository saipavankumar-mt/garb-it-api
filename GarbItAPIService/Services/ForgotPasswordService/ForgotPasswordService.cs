using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgotPasswordService
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private IForgotPasswordProvider _forgotPasswordProvider;
        private IAdminProvider _adminProvider;

        public ForgotPasswordService(IForgotPasswordProvider forgotPasswordProvider, IAdminProvider adminProvider)
        {
            _forgotPasswordProvider = forgotPasswordProvider;
            _adminProvider = adminProvider;
        }

        public async Task<AddSecretQuestionResponse> AddSecretQuestionAsync(SecretQuestionAddRequest req)
        {
            return await _forgotPasswordProvider.AddSecretQuestionAsync(req);
        }


        public async Task<List<SecretQuestion>> GetSecretQuestionsAsync()
        {
            return await _forgotPasswordProvider.GetSecretQuestionsAsync();
        }

        public async Task<SuccessResponse> ForgotPasswordChangeRequestAsync(ForgotPasswordChangeRequest req)
        {
            var userinfo = await _adminProvider.GetAdminInfoAsync(req.Id);
            if (userinfo != null)
            {
                var qIds = userinfo.SecretQuestions;
                var ans = userinfo.SecretAnswers;

                if (AreEqual(ans, req.Answers))
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
