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
    public class AdminController : ControllerBase
    {
        private IAdminService _adminService; 
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var result = await _adminService.GetAdminInfos();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(string id)
        {
            var result = await _adminService.GetAdminInfoAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody]AdminInfo adminInfo)
        {
            var result = await _adminService.AddAdmin(adminInfo);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var result = await _adminService.RemoveAdminInfoByIdAsync(id);
            return Ok(result);
        }
    }
}
