using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Models
{
    public class ApiResponse
    {
        public Status Status { get; set; }
    }

    public enum Status
    {
        Success,

        Failure
    }
}
