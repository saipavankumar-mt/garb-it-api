﻿using AWSDynamoDBProvider.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider
{
    public static class AdminInfoTranslator
    {
        public static AdminInfo ToDBModel(this Contracts.Models.AdminInfo req, string nextId) 
        {
            var dbModel = new AdminInfo()
            {
                AdminId = nextId,
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                ReportsToId = req.ReportsToId,
                ReportsToName = req.ReportsToName,
                Designation = req.Designation,
                Department = req.Department,
                Location = req.Location,
                Muncipality = req.Muncipality,
                City = req.City,
                State = req.State
            };

            return dbModel;
        }
    }
}
