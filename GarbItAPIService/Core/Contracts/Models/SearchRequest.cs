using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class SearchRequest
    {
        public string SearchByKey { get; set; }
        public string SearchByValue { get; set; }
    }
}
