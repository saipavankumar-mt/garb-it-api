using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class AdminInfoTranslator
    {
        public static AdminInfo ToDBModel(this Contracts.Models.AdminInfo req, string nextId = "") 
        {
            var dbModel = new AdminInfo()
            {
                Id = string.IsNullOrEmpty(req.Id) ? nextId : req.Id,
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                Role = req.Role.ToString(),
                ReportsToId = req.ReportsToId,
                ReportsToName = req.ReportsToName,
                Designation = req.Designation,
                Department = req.Department,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                CreatedById = req.CreatedById,
                CreatedByName = req.CreatedByName,
                CreatedDateTime = req.CreatedDateTime,
                UpdatedById = req.UpdatedById,
                UpdatedByName = req.UpdatedByName,
                UpdatedDateTime = req.UpdatedDateTime
            };

            return dbModel;
        }
    }
}
