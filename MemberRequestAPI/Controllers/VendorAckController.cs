using Microsoft.AspNetCore.Mvc;
using MemberRequestAPI.Services;

namespace MemberRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorAckController : ControllerBase
    {
        private readonly VendorAckService _vendorAckService;
        private readonly ILogger<VendorAckController> _logger;

        public VendorAckController(VendorAckService vendorAckService, ILogger<VendorAckController> logger)
        {
            _vendorAckService = vendorAckService;
            _logger = logger;
        }

        [HttpPost("process-ack-file")]
        public async Task<IActionResult> ProcessAckFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file uploaded" });
            }

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                var content = await reader.ReadToEndAsync();
                var result = await _vendorAckService.ProcessVendorAckFile(file.FileName, content);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing vendor acknowledgment file");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
