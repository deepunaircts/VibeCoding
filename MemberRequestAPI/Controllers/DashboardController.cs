using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MemberRequestAPI.Models.Dashboard;
using MemberRequestAPI.Services;
using MemberRequestAPI.Data.Repositories;
using MemberRequestAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MemberRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;

        public DashboardController(DashboardService dashboardService, ILogger<DashboardController> logger, ApplicationDbContext context)
        {
            _dashboardService = dashboardService;
            _logger = logger;
            _context = context;
        }

        [HttpGet("delayed-requests")]
        public async Task<IActionResult> GetDelayedRequests(
            [FromQuery(Name = "sortOrder")] string sortOrder = "CreatedDate_desc",
            [FromQuery(Name = "pageSize")] int? pageSize = null,
            [FromQuery(Name = "pageNumber")] int? pageNumber = null)
        {
            try
            {
                var requests = await _dashboardService.GetDelayedRequestsAsync(sortOrder, pageSize, pageNumber);
                var total = await _dashboardService.GetTotalDelayedRequestsAsync();

                return Ok(new
                {
                    data = requests,
                    total
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting delayed requests");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = new
                {
                    totalDelayedRequests = await _dashboardService.GetTotalDelayedRequestsAsync(),
                    processedRequests = await _context.MemberRequests.CountAsync(r => r.Status == "Processed"),
                    completedRequests = await _context.MemberRequests.CountAsync(r => r.Status == "Completed"),
                    averageDaysAwaitingAck = await _context.MemberRequests
                        .Where(r => r.Status == "Processed" && !r.VendorAckDate.HasValue)
                        .AverageAsync(r => (DateTime.UtcNow - r.CreatedDate).TotalDays)
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
