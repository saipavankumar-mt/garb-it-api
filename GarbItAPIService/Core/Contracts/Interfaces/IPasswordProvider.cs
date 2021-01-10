
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IPasswordProvider
    {
        Task<bool> AddUserToRegistry(PasswordInfo passwordInfo);

        Task<PasswordInfo> GetUserPassword(string userName);

        Task<bool> RemoveUserFromPasswordRegistry(string userName);

        Task<bool> UpdatePasswordAsync(PasswordInfo passwordInfo);
    }
}
