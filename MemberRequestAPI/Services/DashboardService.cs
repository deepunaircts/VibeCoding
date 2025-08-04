using System.Linq.Dynamic.Core;
using MemberRequestAPI.Data.Repositories;
using MemberRequestAPI.Models;
using MemberRequestAPI.Models.Dashboard;

namespace MemberRequestAPI.Services
{
    public class DashboardService
    {
        private readonly IMemberRequestRepository _repository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(IMemberRequestRepository repository, ILogger<DashboardService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<MemberRequestDashboard>> GetDelayedRequestsAsync(string sortOrder = "CreatedDate_desc", int? pageSize = null, int? pageNumber = null)
        {
            try
            {
                // Get requests that are processed but not acknowledged
                var requests = await _repository.GetMemberRequestsByStatusAsync("Processed");
                
                var dashboardItems = requests
                    .Where(r => !r.VendorAckDate.HasValue)
                    .Select(r => new MemberRequestDashboard
                    {
                        RequestId = r.RequestId,
                        MemberId = r.MemberId,
                        LastName = r.LastName,
                        FirstName = r.FirstName,
                        FullAddress = $"{r.Address1}{(string.IsNullOrEmpty(r.Address2) ? "" : ", " + r.Address2)}, {r.City}, {r.State} {r.Zipcode}",
                        RequestType = r.RequestType,
                        Language = r.Language,
                        CreatedDate = r.CreatedDate,
                        DaysAwaitingAck = (int)(DateTime.UtcNow - r.CreatedDate).TotalDays,
                        RequestFileName = r.RequestFileName
                    })
                    .OrderBy(sortOrder)
                    .Skip((pageNumber ?? 1) * (pageSize ?? 10))
                    .Take(pageSize ?? 10)
                    .ToList();

                // Filter requests that have been waiting more than a week
                var delayedRequests = requests
                    .Where(r => r.DaysAwaitingAck > 7)
                    .ToList();

                return delayedRequests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delayed requests");
                throw;
            }
        }

        public async Task<int> GetTotalDelayedRequestsAsync()
        {
            try
            {
                return await _repository.GetTotalDelayedRequestsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting delayed requests");
                throw;
            }
        }
    }
}
