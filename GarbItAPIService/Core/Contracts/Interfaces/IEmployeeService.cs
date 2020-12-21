﻿using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeInfo>> GetEmployees();

        Task<bool> AddEmployee(EmployeeInfo employeeInfo);
    }
}
