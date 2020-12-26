using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IRecordEntryService
    {
        Task<bool> AddRecordEntryAsync(string qrCodeId);
    }
}
