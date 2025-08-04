using System.Text;
using MemberRequestAPI.Data.Repositories;
using MemberRequestAPI.Models;

namespace MemberRequestAPI.Services
{
    public class ExportService
    {
        private readonly IMemberRequestRepository _repository;

        public ExportService(IMemberRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<(string FilePath, string FileName)> GenerateMemberRequestsCsv()
        {
            var requests = await _repository.GetMemberRequestsByStatusAsync("Pending");

            // Generate a unique filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"MemberRequests_{timestamp}.csv";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Exports", fileName);

            // Ensure the Exports directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the CSV file
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Add CSV header
                await writer.WriteLineAsync("MemberID,LastName,FirstName,Address1,Address2,City,State,Zipcode,RequestType,Language,CreatedDate");

                // Add data rows
                foreach (var request in requests)
                {
                    // Escape any commas in the data by wrapping in quotes
                    var line = new List<string>
                    {
                        EscapeCsvField(request.MemberId),
                        EscapeCsvField(request.LastName),
                        EscapeCsvField(request.FirstName),
                        EscapeCsvField(request.Address1),
                        EscapeCsvField(request.Address2 ?? string.Empty),
                        EscapeCsvField(request.City),
                        EscapeCsvField(request.State),
                        EscapeCsvField(request.Zipcode),
                        EscapeCsvField(request.RequestType),
                        EscapeCsvField(request.Language),
                        request.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    await writer.WriteLineAsync(string.Join(",", line));
                }

                foreach (var request in requests)
                {
                    request.Status = "Processed";
                    await _repository.UpdateMemberRequestAsync(request);
                }
            }

            return (filePath, fileName);
        }

        private string EscapeCsvField(string field)
        {
            if (field == null) return string.Empty;
            
            // If the field contains commas, quotes, or newlines, wrap it in quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                // Double up any quotes in the field
                field = field.Replace("\"", "\"\"");
                // Wrap the field in quotes
                return $"\"{field}\"";
            }
            return field;
        }
    }
}
