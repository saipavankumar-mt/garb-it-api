using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class ForgotPasswordChangeRequest
    {
        public string Id { get; set; }
        public List<string> QuestionIds { get; set; }
        public List<string> Answers { get; set; }        
        public string NewPassword { get; set; }
    }
}
