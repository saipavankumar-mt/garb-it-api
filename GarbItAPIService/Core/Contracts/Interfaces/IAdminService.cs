using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminInfo>> GetAdminInfos();

        Task<AddUserResponse> AddAdmin(AdminAddRequest req);

        Task<AdminInfo> GetAdminInfoAsync(string id);
        Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id);
        Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo);
        Task<CountResponse> GetAdminsCountAsync();
        Task<SuccessResponse> UpdateAdminPasswordAsync(UpdatePasswordRequest req);
        Task<SuccessResponse> UpdateSecretQuestionsAsync(AddUserSecretQuestionsRequest req);
        Task<List<SecretQuestion>> GetUserSecretQuestionsAsync(string id);
    }
}
