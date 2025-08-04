using Microsoft.AspNetCore.Mvc;
using MemberRequestAPI.Services;

namespace MemberRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ExportService _exportService;
        private readonly ILogger<ExportController> _logger;

        public ExportController(ExportService exportService, ILogger<ExportController> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        [HttpPost("export-requests")]
        public async Task<IActionResult> ExportRequests()
        {
            try
            {
                var (filePath, fileName) = await _exportService.GenerateMemberRequestsCsv();
                
                // Check if file was created
                if (!System.IO.File.Exists(filePath))
                {
                    return StatusCode(500, "Error generating export file");
                }

                // Read the file and return it as a file download
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting member requests");
                return StatusCode(500, "An error occurred while exporting member requests");
            }
        }
    }
}
