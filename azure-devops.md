<!--
  Use this file for workspace-specific Copilot instructions.
  Learn more: https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file
-->

# Azure DevOps MCP Server in .NET

A Model Context Protocol (MCP) server built with .NET 8+ that integrates with Azure DevOps to query, create, update, and delete work items via the Azure DevOps REST API and .NET SDK.

## Key Features

1. **@modelcontextprotocol/sdk** for seamless MCP integration
2. **Microsoft.TeamFoundationServer.Client** & **Azure.DevOps** .NET SDK for robust DevOps operations
3. **C#** with .NET 8+, nullable reference types, and best practices
4. Comprehensive error handling & input validation with **FluentValidation**
5. Configuration via `appsettings.json` & environment variables
6. Built-in logging with **Serilog**, telemetry, and retry policies via **Polly**
7. Unit tested with **NUnit** and **Moq**

## Getting Started

### Prerequisites

- .NET 8+ SDK
- Azure DevOps Personal Access Token (PAT)
- Environment variables:
  - `AZURE_DEVOPS_ORG_URL`
  - `AZURE_DEVOPS_PROJECT`
  - `AZURE_DEVOPS_TOKEN`

### Installation & Setup

```bash
git clone https://github.com/yourOrg/azureDevops-mcp-dotnet.git
cd azureDevops-mcp-dotnet
dotnet restore
```

### Configuration

Add your settings in `appsettings.json` or set environment variables:

appsettings.json:

```json
{
  "AzureDevOps": {
    "OrganizationUrl": "https://dev.azure.com/yourOrg",
    "ProjectName": "yourProject",
    "PatToken": ""
  }
}
```

Or via env:

```bash
export AZURE_DEVOPS_ORG_URL=https://dev.azure.com/yourOrg
export AZURE_DEVOPS_PROJECT=yourProject
export AZURE_DEVOPS_TOKEN=yourPAT
```

### Run

```bash
dotnet build
dotnet run --project src/AzureDevOps.McpServer
```

## Development Guidelines

- Enable `<Nullable>enable</Nullable>` & `<ImplicitUsings>enable</ImplicitUsings>` in your `.csproj`
- Use `async/await` with `try/catch` + return `Task`/`Task<T>`
- Validate payloads with FluentValidation or DataAnnotations
- Adhere to Azure best practices (invoke `azure_development-get_best_practices`)
- Enforce standards with EditorConfig & StyleCop
- Write tests with xUnit & Moq
- CI/CD via GitHub Actions & SonarCloud

For full protocol details & examples:  
https://modelcontextprotocol.io/llms-full.txt
