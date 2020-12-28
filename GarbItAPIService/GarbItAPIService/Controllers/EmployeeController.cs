using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbItAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseAPIController
    {
        private IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Can be accessed by Admin to view Employees under him
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync([FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.GetEmployees();
            return Ok(result);
        }

        /// <summary>
        /// Can be access by SuperAdmin to view all employees under all admins 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployeesAsync([FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.GetAllEmployees();
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by Admin to Get employee info by EmployeeId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.GetEmployeeInfoAsync (id);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by Admin to Add an employee under him
        /// </summary>
        /// <param name="employeeAddRequest"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeAddRequest employeeAddRequest, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.AddEmployee(employeeAddRequest);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by Admin to Update an employee under him
        /// </summary>
        /// <param name="employeeInfo"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeInfo employeeInfo, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.UpdateEmployeeAsync(employeeInfo);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by Admin to remove an employee under him by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEmployee(string id, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _employeeService.RemoveEmployeeInfoByIdAsync(id);
            return Ok(result);
        }
    }
}
