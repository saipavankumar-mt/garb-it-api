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
    public class LoginController : ControllerBase
    {
        private ISessionService _sessionService;
        
        public LoginController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSessionAsync([FromBody] LoginRequest loginRequest)
        {
            var session = await _sessionService.CreateSessionAsync(loginRequest);
            return Ok(session);
        }
    }
}
