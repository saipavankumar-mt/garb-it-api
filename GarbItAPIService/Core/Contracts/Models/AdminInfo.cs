using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class AdminInfo : UserInfo
    {
        public AdminInfo()
        {
            SecretQuestions = new List<string>();
            SecretAnswers = new List<string>();
        }

        public List<string> SecretQuestions { get; set; }
        public List<string> SecretAnswers { get; set; }
        public string ReportsToId { get; set; }
        public string ReportsToName { get; set; }
    }
}
