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

        /// <summary>
        /// Can be accessed by Admin to Create a client in his location
        /// </summary>
        /// <param name="clientAddRequest"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterClientAsync([FromBody] ClientAddRequest clientAddRequest, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _clientService.RegisterClientAsync(clientAddRequest);
            return Ok(result);
        }

        /// <summary>
        /// Accessed by Employee to scan QRCode
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet("{qrCodeId}")]
        public async Task<IActionResult> GetClientInfoAsync(string qrCodeId, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _clientService.GetClientInfoAsync(qrCodeId);
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchClientAsync([FromHeader(Name = "session-key")] string sessionKey, [FromBody] List<SearchRequest> searchRequests)
        {
            var result = await _clientService.SearchClientAsync(searchRequests);
            return Ok(result);
        }

        /// <summary>
        /// Accessed by Admin to Update Client info
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateClientAsync([FromBody] ClientInfo clientInfo, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _clientService.UpdateClientAsync(clientInfo);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetClientsCountAsync([FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _clientService.GetClientsCountAsync();
            return Ok(result);
        }
    }
}
