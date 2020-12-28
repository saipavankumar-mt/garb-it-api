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
    public class SuperAdminController : BaseAPIController
    {
        private ISuperAdminService _superAdminService; 

        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        /// <summary>
        /// Used only at time of project setup / Installation
        /// </summary>
        /// <param name="superAdminAddRequest"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSuperAdminAsync(SuperAdminAddRequest superAdminAddRequest, [FromHeader(Name = "access-key")] string accessKey)
        {
            var result = await _superAdminService.AddSuperAdminAsync(superAdminAddRequest);
            return Ok(result);
        }

        /// <summary>
        /// Can be accessed by SuperAdmin to view superAdmin profile
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSuperAdminInfoAsync([FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _superAdminService.GetSuperAdminInfoAsync();
            return Ok(result);
        }
    }
}
