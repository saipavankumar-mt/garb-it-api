using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Threading.Tasks;

namespace RecordEntryService
{
    public class RecordEntryService : IRecordEntryService
    {
        private IRecordEntryProvider _recordEntryProvider;
        private IClientProvider _clientProvider;

        public RecordEntryService(IRecordEntryProvider recordEntryProvider, IClientProvider clientProvider)
        {
            _recordEntryProvider = recordEntryProvider;
            _clientProvider = clientProvider;
        }

        public async Task<AddRecordResponse> AddRecordEntryAsync(string qrCodeId)
        {
            //Get clientInfo from QR Code ID

            var clientInfo = await _clientProvider.GetClientInfoAsync(qrCodeId);

            //Get Employee Info from Session key

            var recordInfo = new RecordEntryInfo()
            {
                ClientId = clientInfo.ClientId,
                ClientName = clientInfo.Name,
                EmployeeId = AmbientContext.Current.UserId,
                EmployeeName = AmbientContext.Current.UserName,
                Location = clientInfo.Location
            };

            return await _recordEntryProvider.AddRecordEntryAsync(recordInfo);
        }
    }
}
