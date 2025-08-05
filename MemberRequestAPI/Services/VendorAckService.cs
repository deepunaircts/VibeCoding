using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using MemberRequestAPI.Data.Repositories;
using MemberRequestAPI.Models;

namespace MemberRequestAPI.Services
{
    public class VendorAckService
    {
        private readonly IMemberRequestRepository _repository;
        private readonly ILogger<VendorAckService> _logger;

        public VendorAckService(IMemberRequestRepository repository, ILogger<VendorAckService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> ProcessVendorAckFile(string fileName, string content)
        {
            try
            {
                // Parse the CSV content
                var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var request = new MemberRequest();
                // Skip header line
                var headers = lines[0].Split(',');
                var dataLines = lines.Skip(1).ToList();

                foreach (var line in dataLines)
                {
                    var fields = line.Split(',');
                    
                    if (fields.Length < 3) continue; // Skip invalid lines

                    // Map the fields
                    var memberId = fields[0].Trim('"');
                    var vendorAckDate = DateTime.ParseExact(fields[1].Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var mailingDate = DateTime.ParseExact(fields[2].Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    // Find the corresponding request
                    request = await _repository.GetMemberRequestByIdAsync(int.Parse(memberId));

                    if (request != null)
                    {
                        request.VendorAckDate = vendorAckDate;
                        request.MailingDate = mailingDate;
                        request.VendorAckFileName = fileName;
                        request.Status = "Completed";
                        _logger.LogInformation($"Updated request {request.RequestId} with vendor acknowledgment");
                    }
                    else
                    {
                        _logger.LogWarning($"No matching request found for MemberID: {memberId}");
                    }
                }

                // Save changes
                await _repository.UpdateMemberRequestAsync(request);

                return new OkObjectResult(new { message = "Vendor acknowledgment file processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing vendor acknowledgment file");
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }
    }
}
