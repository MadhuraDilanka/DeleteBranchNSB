# Delete Branch NSB

A .NET 8 Windows Forms desktop tool that connects to a SQL Server database and deletes documents for a selected branch across multiple libraries.

## Features

- Enter any SQL Server connection string and connect instantly
- Loads **Libraries** from the `Portal` table (`ID`, `Name`, `IndexDataTable`)
- Loads **Branches** from the `TempRefTable` table (`[Branch Name]`)
- Multi-select libraries via a checkbox list (Select All / Clear All helpers)
- For each selected library, reads the `IndexDataTable` GUID (dynamic SQL table name)
- Queries `SELECT DocumentID FROM [{GUID}] WHERE [Branch] = @branch`
- Calls `[dbo].[deleteDocument]` stored procedure for every matched document
- Dark console-style operation log with timestamps and progress bar

## Requirements

- .NET 8 SDK (Windows)
- SQL Server with the `Portal`, `TempRefTable`, and GUID-named index tables
- The `[dbo].[deleteDocument]` stored procedure

## Getting Started

```bash
git clone https://github.com/<your-username>/DeleteBranchNSB.git
cd DeleteBranchNSB
dotnet build --configuration Release
dotnet run
```

Or open `DeleteBranchNSB.csproj` in Visual Studio 2022+.

## Connection String Examples

```
# Windows Authentication
Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True;

# SQL Server Authentication
Server=YOUR_SERVER;Database=YOUR_DB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;
```

## Tech Stack

- .NET 8 / Windows Forms
- Microsoft.Data.SqlClient 5.x
