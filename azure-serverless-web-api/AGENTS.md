# AGENTS.md

## Project: Azure Serverless Web API

### Goal
Implement an Azure Serverless Web API using Azure Functions (Isolated Worker Model) and Azure SQL Database with a closed network architecture (Private Endpoints, VNet, Application Gateway + WAF).

### Tech Stack
- **Application**: C# (.NET 9.0), Azure Functions (Isolated Worker), Entity Framework Core
- **Database**: Azure SQL Database
- **Auth/Security**: Azure Key Vault for secrets management (DB connection strings)
- **Infrastructure**: Azure Bicep, PowerShell (Deploy.ps1)
- **Networking**: Azure VNet, Private Endpoints, Application Gateway (Load Balancer + WAF)
- **Testing**: xUnit, Shouldly

### Folder Structure
- `src-app/`: Application source code, including solution (`slnx`), projects, and tests.
- `src-infra/`: Infrastructure-as-Code (Bicep) and deployment scripts.

### Implementation Details
- `AzureServerlessWebApi`: Functions implementation with Key Vault integration.
- `AzureServerlessWebApi.Data`: Data access layer with EF Core.
- `AzureServerlessWebApi.Tests`: Unit tests with Shouldly for readable assertions.
- `GetDatabaseVersion` endpoint: Returns the database version (`SELECT @@Version;`).
- `GetUsers` endpoint: Returns users from `dbo.User`.
