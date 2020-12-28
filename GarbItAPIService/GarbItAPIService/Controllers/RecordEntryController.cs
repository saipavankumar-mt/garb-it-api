using Contracts.Interfaces;
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
    public class RecordEntryController : ControllerBase
    {
        private IRecordEntryService _recordEntryService;

        public RecordEntryController(IRecordEntryService recordEntryService)
        {
            _recordEntryService = recordEntryService;
        }

        [HttpGet("Scan/{qrCodeId}")]
        public async Task<IActionResult> AddScannedRecordAsync(string qrCodeId, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.AddRecordEntryAsync(qrCodeId);
            return Ok(result);
        }
    }
}
