using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Threading.Tasks;

namespace SuperAdminService
{
    public class SuperAdminService : ISuperAdminService
    {
        private ISuperAdminProvider _superAdminProvider;
        public SuperAdminService(ISuperAdminProvider superAdminProvider)
        {
            _superAdminProvider = superAdminProvider;
        }

        public async Task<AddUserResponse> AddSuperAdminAsync(SuperAdminAddRequest req)
        {

            var superAdminInfo = req.ToCoreModel();
            return await _superAdminProvider.AddSuperAdmin(superAdminInfo);
        }


        public async Task<SuperAdminInfo> GetSuperAdminInfoAsync()
        {
            var userId = AmbientContext.Current.UserInfo.Id;

            return await _superAdminProvider.GetSuperAdminInfoAsync(userId);
        }

        public async Task<AddUserResponse> UpdateSuperAdminAsync(SuperAdminInfo superAdminInfo)
        {
            superAdminInfo.UpdatedById = AmbientContext.Current.UserInfo.Id;
            superAdminInfo.UpdatedByName = AmbientContext.Current.UserInfo.Name;
            superAdminInfo.UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            return await _superAdminProvider.UpdateSuperAdminAsync(superAdminInfo);
        }

        public async Task<SuccessResponse> UpdateSuperAdminPasswordAsync(UpdatePasswordRequest req)
        {
            return await _superAdminProvider.UpdateSuperAdminPasswordAsync(req);
        }
    }
}
