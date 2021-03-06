﻿using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class EmployeeInfoTranslator
    {
        public static EmployeeInfo ToDBModel(this Contracts.Models.EmployeeInfo req, string nextId ="")
        {
            var dbModel = new EmployeeInfo()
            {
                Id = string.IsNullOrEmpty(req.Id)? nextId : req.Id,
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                Gender = req.Gender,
                DateOfBirth = req.DateOfBirth,
                Married = req.Married,
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
