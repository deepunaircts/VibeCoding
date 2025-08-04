using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemberEligibilityProcessor.Services
{
    public interface IFileGeneratorService
    {
        Task<string> GenerateEligibilityCsv(IEnumerable<MemberEligibilityReport> reportData);
    }

    public class FileGeneratorService : IFileGeneratorService
    {
        public async Task<string> GenerateEligibilityCsv(IEnumerable<MemberEligibilityReport> reportData)
        {
            var fileName = $"MemberEligibilityReport_{DateTime.Now:yyyyMMddHHmmss}.csv";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            using (var writer = new StreamWriter(filePath))
            {
                // Write header with GroupId
                await writer.WriteLineAsync("MemberID,LastName,FirstName,DateOfBirth,MedicaidID,Address,City,State,ZipCode,GroupID,PlanID,ProductID,EligibilityStatus");
                
                // Write data rows
                foreach (var item in reportData)
                {
                    var line = $"\"{item.MemberId}\"," +
                              $"\"{EscapeCsvField(item.LastName)}\"," +
                              $"\"{EscapeCsvField(item.FirstName)}\"," +
                              $"\"{item.DateOfBirth}\"," +
                              $"\"{EscapeCsvField(item.MedicaidId)}\"," +
                              $"\"{EscapeCsvField(item.Address)}\"," +
                              $"\"{EscapeCsvField(item.City)}\"," +
                              $"\"{EscapeCsvField(item.State)}\"," +
                              $"\"{EscapeCsvField(item.ZipCode)}\"," +
                              $"\"{EscapeCsvField(item.GroupId)}\"," +
                              $"\"{EscapeCsvField(item.PlanId)}\"," +
                              $"\"{EscapeCsvField(item.ProductId)}\"," +
                              $"\"{(item.IsEligible ? "Eligible" : "Not Eligible")}\"";
                    
                    await writer.WriteLineAsync(line);
                }
            }

            return filePath;
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;
                
            // Escape double quotes by doubling them
            return field.Replace("\"", "\"\"");
        }
    }
}
