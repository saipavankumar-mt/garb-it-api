using AWSDynamoDBProvider.Model;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class PasswordInfoTranslator
    {
        public static PasswordRegistry ToDBModel(this PasswordInfo passwordInfo)
        {
            var registry = new PasswordRegistry()
            {
                Id = passwordInfo.Id,
                UserName = passwordInfo.UserName,
                Password = passwordInfo.Password,
                Role = passwordInfo.Role.ToString()
            };

            return registry;
        }

        public static PasswordInfo ToEntityModel(this PasswordRegistry passwordregistry)
        {
            var info = new PasswordInfo()
            {
                Id = passwordregistry.Id,
                UserName = passwordregistry.UserName,
                Password = passwordregistry.Password,
                Role =  Enum.Parse<Role>(passwordregistry.Role)
            };

            return info;
        }
    }
}
