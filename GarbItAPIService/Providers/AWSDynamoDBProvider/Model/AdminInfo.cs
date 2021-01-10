using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class AdminInfo : UserInfo
    {
        public AdminInfo()
        {

        }

        public List<string> SecretQuestions { get; set; }
        public List<string> SecretAnswers { get; set; }
        public string ReportsToId { get; set; }
        public string ReportsToName { get; set; }
        

    }
}
