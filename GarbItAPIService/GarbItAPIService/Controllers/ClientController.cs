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
    public class ClientController : BaseAPIController
    {
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterClientAsync([FromBody] ClientInfo clientInfo)
        {
            var result = await _clientService.RegisterClientAsync(clientInfo);
            return Ok(result);
        }

        [HttpGet("{qrCodeId}")]
        public async Task<IActionResult> GetClientInfoAsync(string qrCodeId)
        {
            var result = await _clientService.GetClientInfoAsync(qrCodeId);
            return Ok(result);
        }


    }
}
