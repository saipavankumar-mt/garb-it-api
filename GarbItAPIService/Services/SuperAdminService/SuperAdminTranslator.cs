using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperAdminService
{
    public static class SuperAdminTranslator
    {
        public static SuperAdminInfo ToCoreModel(this SuperAdminAddRequest req)
        {
            var superAdminInfo = new SuperAdminInfo()
            {
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                Role = req.Role,
                Designation = req.Designation,
                Department = req.Department,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                CreatedById = "Installer",
                CreatedByName = "Installer"
            };

            return superAdminInfo;
        }
    }
}
