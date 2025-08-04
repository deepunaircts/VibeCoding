# Member Eligibility Processor

This application processes member eligibility data, performs eligibility checks, and generates reports for vendors.

## Prerequisites

1. .NET 6.0 SDK or later
2. SQL Server (or another database provider of your choice)
3. Entity Framework Core tools (for database migrations)

## Project Structure

- **Models/**: Contains the data models
  - `Member.cs`: Member information
  - `MemberAddress.cs`: Member address information
  - `MemberEligibility.cs`: Member eligibility information
- **Data/**: Data access layer
  - `MemberDbContext.cs`: Database context for Entity Framework Core
- **Services/**: Business logic and file generation
  - `IEligibilityService.cs` and `EligibilityService.cs`: Handles eligibility logic
  - `IFileGeneratorService.cs` and `FileGeneratorService.cs`: Handles CSV file generation
- **Program.cs**: Application entry point

## Setup Instructions

1. **Update Connection String**:
   - Open `Program.cs`
   - Replace `"YourConnectionStringHere"` with your actual database connection string

2. **Create Database and Apply Migrations**:
   ```bash
   cd c:\Balaji\WindSurf_Demo
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```

## How It Works

1. The application queries the database for eligible members based on:
   - Current date is between the effective and termination dates
   - Eligibility indicator is set to true

2. For each eligible member, it retrieves:
   - Basic member information
   - Home address details
   - Plan and product information

3. The data is then exported to a CSV file in the system's temporary folder with the naming pattern: `MemberEligibilityReport_YYYYMMDDHHMMSS.csv`

## Output File Format

The generated CSV file will contain the following columns:
- MemberID
- LastName
- FirstName
- DateOfBirth
- MedicaidID
- Address
- City
- State
- ZipCode
- PlanID
- ProductID
- EligibilityStatus

## Customization

You can modify the following:
- **Eligibility Criteria**: Update the query in `EligibilityService.GenerateEligibilityReport()`
- **Output Format**: Modify `FileGeneratorService.GenerateEligibilityCsv()`
- **File Naming and Location**: Update the file path generation in `FileGeneratorService`

## Notes

- The application is configured to use SQL Server by default. To use a different database provider, update the `MemberDbContext` configuration in `Program.cs`
- Error handling is basic and should be enhanced for production use
- Consider adding logging for better debugging and monitoring
