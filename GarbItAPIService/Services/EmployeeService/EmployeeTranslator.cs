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
            var userInfo = AmbientContext.Current.UserInfo;
            var employeeInfo = new EmployeeInfo()
            {
                Name = req.Name,
                UserName = req.UserName,
                Password = req.Password,
                PhoneNumber = req.PhoneNumber,
                Gender = req.Gender,
                DateOfBirth = req.DateOfBirth,
                Married = req.Married,
                Role = req.Role,
                Designation = req.Designation,
                Department = req.Department,
                Location = req.Location,
                Municipality = req.Municipality,
                City = req.City,
                State = req.State,
                Country = req.Country,
                ReportsToId = userInfo.Id,
                ReportsToName = userInfo.Name,
                CreatedById = userInfo.Id,
                CreatedByName = userInfo.Name,
                CreatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                UpdatedById = userInfo.Id,
                UpdatedByName = userInfo.Name,
                UpdatedDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
            };

            return employeeInfo;
        }
    }
}
