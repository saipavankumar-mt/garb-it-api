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
            var adminInfo = req.ToCoreModel();
            return await _adminProvider.AddAdmin(adminInfo);
        }


        public async Task<AddUserResponse> UpdateAdminAsync(AdminInfo adminInfo)
        {
            adminInfo.UpdatedById = AmbientContext.Current.UserId;
            adminInfo.UpdatedByName = AmbientContext.Current.UserName;
            adminInfo.UpdatedDateTime = DateTime.Now.ToString();

            return await _adminProvider.UpdateAdminAsync(adminInfo);
        }

        public async Task<AdminInfo> GetAdminInfoAsync(string id)
        {
            return await _adminProvider.GetAdminInfoAsync(id);
        }

        public async Task<List<AdminInfo>> GetAdminInfos()
        {
            var superAdminId = AmbientContext.Current.UserId;
            return await _adminProvider.GetAdmins(superAdminId);
        }

        public async Task<RemoveUserResponse> RemoveAdminInfoByIdAsync(string id)
        {
            return await _adminProvider.RemoveAdminInfoByIdAsync(id);
        }

    }
}
