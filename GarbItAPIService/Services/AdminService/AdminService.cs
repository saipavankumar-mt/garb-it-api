using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService
{
    public class AdminService : IAdminService
    {
        private IAdminProvider _adminProvider;
        public AdminService(IAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        public async Task<AddUserResponse> AddAdmin(AdminAddRequest req)
        {
            var adminInfo = req?.ToCoreModel();
            return await _adminProvider.AddAdmin(adminInfo);
        }


        public async Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo)
        {
            adminInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            adminInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            adminInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            return await _adminProvider.UpdateAdminAsync(adminInfo);
        }

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            return await _adminProvider.GetAdminInfoAsync(id);
        }

        public async Task<List<AdminInfo>> GetAdminInfos()
        {
            var superAdminId = AmbientContext.Current.UserInfo.Id;
            return await _adminProvider.GetAdmins(superAdminId);
        }

        public async Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id)
        {
            return await _adminProvider.RemoveAdminInfoByIdAsync(id);
        }

        public async Task<CountResponse> GetAdminsCountAsync()
        {
            var count = await _adminProvider.GetAdminsCount(AmbientContext.Current.UserInfo.Id);
            return new CountResponse()
            {
                Count = count
            };
        }

        public async Task<SuccessResponse> UpdateAdminPasswordAsync(UpdatePasswordRequest req)
        {
            return await _adminProvider.UpdateAdminPasswordAsync(req);
        }

        public async Task<SuccessResponse> UpdateSecretQuestionsAsync(AddUserSecretQuestionsRequest req)
        {
            return await _adminProvider.UpdateSecretQuestionsAsync(req);
        }

        public async Task<List<SecretQuestion>> GetUserSecretQuestionsAsync(string id)
        {
            return await _adminProvider.GetUserSecretQuestionsAsync(id);
        }
    }
}
