using System.Linq.Dynamic.Core;
using MemberRequestAPI.Data;
using MemberRequestAPI.Models;
using MemberRequestAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MemberRequestAPI.Data.Repositories
{
    public class MemberRequestRepository : IMemberRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MemberRequestRepository> _logger;

        public MemberRequestRepository(ApplicationDbContext context, ILogger<MemberRequestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddMemberRequestAsync(MemberRequest request)
        {
            try
            {
                _context.MemberRequests.Add(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding member request");
                throw;
            }
        }

        public async Task<MemberRequest> GetMemberRequestByIdAsync(int requestId)
        {
            try
            {
                return await _context.MemberRequests.FindAsync(requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting member request {requestId}");
                throw;
            }
        }

        public async Task<IEnumerable<MemberRequest>> GetMemberRequestsAsync()
        {
            try
            {
                return await _context.MemberRequests.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting member requests");
                throw;
            }
        }

        public async Task<IEnumerable<MemberRequest>> GetMemberRequestsByStatusAsync(string status)
        {
            try
            {
                return await _context.MemberRequests
                    .Where(r => r.Status == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting member requests with status {status}");
                throw;
            }
        }

        public async Task<IEnumerable<MemberRequest>> GetMemberRequestsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.MemberRequests
                    .Where(r => r.CreatedDate >= startDate && r.CreatedDate <= endDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting member requests between {startDate} and {endDate}");
                throw;
            }
        }

        public async Task UpdateMemberRequestAsync(MemberRequest request)
        {
            try
            {
                _context.MemberRequests.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating member request {request.RequestId}");
                throw;
            }
        }

        public async Task DeleteMemberRequestAsync(int requestId)
        {
            try
            {
                var request = await _context.MemberRequests.FindAsync(requestId);
                if (request != null)
                {
                    _context.MemberRequests.Remove(request);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting member request {requestId}");
                throw;
            }
        }

        public async Task<int> GetTotalRequestsByStatusAsync(string status)
        {
            try
            {
                return await _context.MemberRequests
                    .CountAsync(r => r.Status == status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error counting requests with status {status}");
                throw;
            }
        }

        public async Task<IEnumerable<MemberRequest>> GetDelayedRequestsAsync()
        {
            try
            {
                return await _context.MemberRequests
                    .Where(r => r.Status == "Processed" && !r.VendorAckDate.HasValue && 
                                (DateTime.UtcNow - r.CreatedDate).TotalDays > 7)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting delayed requests");
                throw;
            }
        }

        public async Task<int> GetTotalDelayedRequestsAsync()
        {
            try
            {
                return await _context.MemberRequests
                    .CountAsync(r => r.Status == "Processed" && !r.VendorAckDate.HasValue && 
                                    (DateTime.UtcNow - r.CreatedDate).TotalDays > 7);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting delayed requests");
                throw;
            }
        }
    }
}
