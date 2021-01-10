using Contracts;
using Contracts.Interfaces;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DashboardService
{
    public class DashboardService : IDashboardService
    {
        private IAdminProvider _adminProvider;
        private IEmployeeProvider _employeeProvider;
        private IClientProvider _clientProvider;
        private IRecordEntryProvider _recordEntryProvider;

        public DashboardService(IAdminProvider adminProvider, IEmployeeProvider employeeProvider, IClientProvider clientProvider, IRecordEntryProvider recordEntryProvider)
        {
            _adminProvider = adminProvider;
            _employeeProvider = employeeProvider;
            _clientProvider = clientProvider;
            _recordEntryProvider = recordEntryProvider;          
        }


        public async Task<DashboardResponse> GetDashboardInfoAsync()
        {
            var dashboardResponse = new DashboardResponse();

            var role = AmbientContext.Current.UserInfo.Role;

            if(role.Equals(Role.SuperAdmin))
            {

                

                var clientCount = await _clientProvider.SearchClientCountAsync(new SearchRequest()
                {
                    SearchByKey = "Location",
                    SearchByValue = AmbientContext.Current.UserInfo.Location
                });
                dashboardResponse.ClientCount = clientCount;

                var searchRequests = new List<SearchRequest>()
                {
                    new SearchRequest(){ SearchByKey = "Location", SearchByValue = AmbientContext.Current.UserInfo.Location }
                };

                var collectedInfo = new CollectedCount();
                collectedInfo.Today = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today, DateTime.Now);
                collectedInfo.Week = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today.AddDays(-7), DateTime.Now);
                collectedInfo.Month = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today.AddMonths(-1), DateTime.Today);

                //Active clients
                var clientsInfo = await _recordEntryProvider.GetCollectedRecordsAsync(searchRequests, DateTime.Today.AddMonths(-1), DateTime.Today);

                var activeClients = 

            }

            if(role.Equals(Role.Admin))
            {
               

                var clientCount = await _clientProvider.SearchClientCountAsync(new SearchRequest() { 
                    SearchByKey = "Municipality",
                    SearchByValue = AmbientContext.Current.UserInfo.Municipality
                });
                dashboardResponse.ClientCount = clientCount;

                var searchRequests = new List<SearchRequest>()
                {
                    new SearchRequest(){ SearchByKey = "Municipality", SearchByValue = AmbientContext.Current.UserInfo.Municipality }
                };

                var collectedInfo = new CollectedCount();
                collectedInfo.Today = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today, DateTime.Now);
                collectedInfo.Week = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today.AddDays(-7), DateTime.Now);
                collectedInfo.Month = await _recordEntryProvider.GetCollectedCountAsync(searchRequests, DateTime.Today.AddMonths(-1), DateTime.Today);
            }

            return dashboardResponse;
        }
    }
}
