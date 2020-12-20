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

        public Task<bool> AddAdmin(AdminInfo adminInfo)
        {
            return _adminProvider.AddAdmin(adminInfo);
        }

        public Task<List<AdminInfo>> GetAdminInfos()
        {
            return _adminProvider.GetAdmins();
        }
    }
}
