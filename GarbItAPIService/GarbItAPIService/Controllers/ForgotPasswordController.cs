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
    public class ForgotPasswordController : BaseAPIController
    {
        private IForgotPasswordService _forgotPasswordService;
        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpGet("secretquestions")]
        public async Task<IActionResult> GetSecretQuestionsAsync()
        {
            var result = await _forgotPasswordService.GetSecretQuestionsAsync();
            return Ok(result);
        }

        [HttpPost("addsecretquestions")]
        public async Task<IActionResult> AddSecretQuestionAsync([FromBody] SecretQuestionAddRequest req, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _forgotPasswordService.AddSecretQuestionAsync(req);
            return Ok(result);
        }


        [HttpPost("change")]
        public async Task<IActionResult> ForgotPasswordChangeRequestAsync([FromBody] ForgotPasswordChangeRequest req, [FromHeader(Name = "session-key")] string sessionKey)
        {
            var result = await _forgotPasswordService.ForgotPasswordChangeRequestAsync(req);
            return Ok(result);
        }
    }
}
