using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class SuperAdminInfoTranslator
    {
        public static SuperAdminInfo ToDBModel(this Contracts.Models.SuperAdminInfo req, string nextId="")
        {
            var dbModel = new SuperAdminInfo()
            {
                Id = string.IsNullOrEmpty(req.Id) ? nextId : req.Id,
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                Role = req.Role.ToString(),
                Designation = req.Designation,
                Department = req.Department,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                CreatedById = req.CreatedById,
                CreatedByName = req.CreatedByName,
                CreatedDateTime = DateTime.Now.ToString()                
            };

            return dbModel;
        }
    }
}
