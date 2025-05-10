using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol;
using Serilog;
using Serilog.Events;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

// Add configuration
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configure logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
    loggingBuilder.AddSerilog(dispose: true));

// Add MCP server with STDIO transport
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Add Azure DevOps client
builder.Services.AddSingleton<WorkItemTrackingHttpClient>(sp =>
{
    var pat = Environment.GetEnvironmentVariable("AZURE_DEVOPS_TOKEN") ??
        throw new InvalidOperationException("AZURE_DEVOPS_TOKEN environment variable is not set");

    var orgUrl = Environment.GetEnvironmentVariable("AZURE_DEVOPS_ORG_URL") ??
        throw new InvalidOperationException("AZURE_DEVOPS_ORG_URL environment variable is not set");
    if (string.IsNullOrEmpty(orgUrl))
        throw new InvalidOperationException("Azure DevOps OrganizationUrl is not configured");

    var connection = new VssConnection(
        new Uri(orgUrl),
        new VssBasicCredential(string.Empty, pat));

    return connection.GetClient<WorkItemTrackingHttpClient>();
});

var app = builder.Build();

await app.RunAsync();
