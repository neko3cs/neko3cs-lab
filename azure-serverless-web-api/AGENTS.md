# AGENTS.md

## Project: Azure Serverless Web API

### Goal

Implement an Azure Serverless Web API using Azure Functions (Isolated Worker Model) and Azure SQL Database with a closed network architecture (Private Endpoints, VNet, Application Gateway + WAF).

### Tech Stack

- **Application**: C# (.NET 10.0), Azure Functions (Isolated Worker), Entity Framework Core
- **Database**: Azure SQL Database
- **Auth/Security**: Middleware-based API Key Authentication (`X-API-KEY` header). **Note: Azure Key Vault integration is currently suspended due to RBAC deployment issues.**
- **Infrastructure**: Azure Bicep (Modularized: Network, Database, Application, AppGateway), PowerShell (Deploy.ps1)
- **Networking**: Azure VNet, Private Endpoints (SQL), Application Gateway (Load Balancer + WAF)
- **Testing**: xUnit, Shouldly, Moq (with custom HTTP Fakes)

### Folder Structure

- `src-app/`: .NET 10 source code.
  - `AzureServerlessWebApi/`: Functions and Middleware.
  - `AzureServerlessWebApi.Data/`: EF Core layer.
  - `AzureServerlessWebApi.Tests/`: Unit tests organized by namespace.
- `src-infra/`: Bicep modules and deployment scripts.

### Current Status

- ✅ Infrastructure deployment via Bicep (VNet, SQL, App Service Plan, App Gateway) is fully functional.
- ✅ Function App deployment via `func publish` is successful.
- ✅ API Endpoints (`GetDatabaseVersion`, `GetUsers`) are verified and working.
- ✅ Automatic DB schema creation and seeding (5 sample users) is implemented.
- ⚠️ Key Vault integration is blocked by repeated `RoleDefinitionDoesNotExist` errors during Bicep deployment.
