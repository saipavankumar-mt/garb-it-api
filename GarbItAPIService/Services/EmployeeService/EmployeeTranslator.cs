using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeService
{
    public static class EmployeeTranslator
    {
        public static EmployeeInfo ToCoreModel(this EmployeeAddRequest req)
        {
            var employeeInfo = new EmployeeInfo()
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
                ReportsToId = AmbientContext.Current.UserId,
                ReportsToName = AmbientContext.Current.UserName,
                CreatedById = AmbientContext.Current.UserId,
                CreatedByName = AmbientContext.Current.UserName,
                CreatedDateTime = DateTime.Now.ToString(),
                UpdatedById = AmbientContext.Current.UserId,
                UpdatedByName = AmbientContext.Current.UserName,
                UpdatedDateTime = DateTime.Now.ToString(),
            };

            return employeeInfo;
        }
    }
}
