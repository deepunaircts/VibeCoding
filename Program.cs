using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MemberEligibilityProcessor.Data;
using MemberEligibilityProcessor.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        try
        {
            Console.WriteLine("Starting Member Eligibility Report Generation...");
            
            // Get the services and run the report generation
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var eligibilityService = services.GetRequiredService<IEligibilityService>();
                var fileGenerator = services.GetRequiredService<IFileGeneratorService>();
                
                // Generate the report data
                var reportData = await eligibilityService.GenerateEligibilityReport();
                
                // Generate the CSV file
                var filePath = await fileGenerator.GenerateEligibilityCsv(reportData);
                
                Console.WriteLine($"Report generated successfully at: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Configure DbContext with connection string (should be in appsettings.json in a real app)
                services.AddDbContext<MemberDbContext>(options =>
                    options.UseSqlServer("YourConnectionStringHere"));
                
                // Register services
                services.AddScoped<IEligibilityService, EligibilityService>();
                services.AddScoped<IFileGeneratorService, FileGeneratorService>();
            });
}
