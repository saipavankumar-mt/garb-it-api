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
    public class RecordEntryController : BaseAPIController
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

        [HttpPost("search")]
        public async Task<IActionResult> SearchRecordAsync([FromQuery]string fromDate, [FromQuery] string toDate, [FromBody] List<SearchRequest> searchRequests, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.SearchRecordAsync(searchRequests, fromDate, toDate);
            return Ok(result);
        }

        [HttpPost("count")]
        public async Task<IActionResult> GetScannedRecordsCountAsync([FromQuery] string fromDate, [FromQuery] string toDate, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.GetScannedRecordsCountAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpPost("daycount")]
        public async Task<IActionResult> GetScannedRecordsDayCountAsync([FromQuery] string fromDate, [FromQuery] string toDate, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.GetScannedRecordsDayCountAsync(fromDate, toDate);
            return Ok(result);
        }

        [HttpGet("activeclients")]
        public async Task<IActionResult> GetActiveClientsCountAsync([FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.GetActiveClientsCountAsync();
            return Ok(result);
        }

        [HttpGet("employeescannedcounts")]
        public async Task<IActionResult> GetEmployeesScannedCountAsync([FromQuery] string fromDate, [FromQuery] string toDate, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _recordEntryService.GetEmployeesScannedCountAsync(fromDate, toDate);
            return Ok(result);
        }
    }
}
