using System;
using System.Collections.Generic;
using System.Text;

namespace AWSDynamoDBProvider.Model
{
    public class DBBase
    {
        public string CreatedDateTime { get; set; }
        public string UpdatedDateTime { get; set; }

        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
    }
}
