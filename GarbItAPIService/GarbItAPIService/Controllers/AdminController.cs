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
    [ApiController()]    
    public class AdminController : BaseAPIController
    {
        private IAdminService _adminService; 
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Can be accessed only by SuperAdmin to view available admins who reports to him
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet]           
        public async Task<IActionResult> GetAdminsAsync([FromHeader(Name ="session-key")] string sessionKey)
        {
            var result = await _adminService.GetAdminInfos();
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed only by SuperAdmin to view Specific Admin with Admin Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminByIdAsync(string id, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _adminService.GetAdminInfoAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed only by SuperAdmin to create admin for location
        /// </summary>
        /// <param name="adminAddRequest"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddAdminAsync([FromBody] AdminAddRequest adminAddRequest, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _adminService.AddAdmin(adminAddRequest);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by admin to update his info
        /// </summary>
        /// <param name="adminInfo"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAdminAsync([FromBody]AdminInfo adminInfo, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _adminService.UpdateAdminAsync(adminInfo);
            return Ok(result);
        }


        /// <summary>
        /// Can be accessed only by Super Admin to delete particular Admin by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAdmin(string id, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _adminService.RemoveAdminInfoByIdAsync(id);
            return Ok(result);
        }
    }
}
