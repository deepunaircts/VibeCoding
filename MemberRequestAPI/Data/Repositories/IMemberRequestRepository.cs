using MemberRequestAPI.Models;
using System.Collections.Generic;

namespace MemberRequestAPI.Data.Repositories
{
    public interface IMemberRequestRepository
    {
        Task AddMemberRequestAsync(MemberRequest request);
        Task<MemberRequest> GetMemberRequestByIdAsync(int requestId);
        Task<IEnumerable<MemberRequest>> GetMemberRequestsAsync();
        Task<IEnumerable<MemberRequest>> GetMemberRequestsByStatusAsync(string status);
        Task<IEnumerable<MemberRequest>> GetMemberRequestsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task UpdateMemberRequestAsync(MemberRequest request);
        Task DeleteMemberRequestAsync(int requestId);
        Task<int> GetTotalRequestsByStatusAsync(string status);
        Task<IEnumerable<MemberRequest>> GetDelayedRequestsAsync();
        Task<int> GetTotalDelayedRequestsAsync();
    }
}
