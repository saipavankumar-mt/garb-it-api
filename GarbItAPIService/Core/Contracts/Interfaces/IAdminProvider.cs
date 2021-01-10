using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IAdminProvider
    {
        Task<List<AdminInfo>> GetAdmins(string reportsToId = "");

        Task<AddUserResponse> AddAdmin(AdminInfo adminInfo);

        Task<AdminInfo> GetAdminInfoAsync(string id);
        Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id);
        Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo);

        Task<int> GetAdminsCount(string reportsToId = "");
        Task<SuccessResponse> UpdateAdminPasswordAsync(UpdatePasswordRequest req);
        Task<SuccessResponse> UpdateSecretQuestionsAsync(AddUserSecretQuestionsRequest req);
        Task<List<SecretQuestion>> GetUserSecretQuestionsAsync(string id);
    }
}
