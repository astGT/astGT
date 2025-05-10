using System.ComponentModel;
using FluentValidation;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using ModelContextProtocol.Server;
using Serilog;

namespace AzureDevOpsMcp.Tools;

[McpServerToolType]
public static class WorkItemTools
{
    [McpServerTool, Description("Get the description and details of a work item")]
    public static async Task<string> GetWorkItemDescription(
        WorkItemTrackingHttpClient workItemClient,
        [Description("The ID of the work item to get details for")]
        int workItemId)
    {
        try
        {
            var workItem = await workItemClient.GetWorkItemAsync(workItemId);
            if (workItem == null)
                return $"Work item {workItemId} not found.";

            return FormatWorkItemDetails(workItem);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting work item {WorkItemId}", workItemId);
            return $"Error getting work item {workItemId}: {ex.Message}";
        }
    }

    [McpServerTool, Description("Generate a plan to resolve a work item")]
    public static async Task<string> GeneratePlanForWorkItem(
        WorkItemTrackingHttpClient workItemClient,
        [Description("The ID of the work item to generate a plan for")]
        int workItemId)
    {
        try
        {
            var workItem = await workItemClient.GetWorkItemAsync(workItemId);
            if (workItem == null)
                return $"Work item {workItemId} not found.";

            // Get the description and acceptance criteria
            var description = workItem.Fields != null && workItem.Fields.ContainsKey("System.Description")
                ? workItem.Fields["System.Description"]?.ToString() ?? string.Empty
                : string.Empty;

            var acceptanceCriteria = workItem.Fields != null && workItem.Fields.ContainsKey("Microsoft.VSTS.Common.AcceptanceCriteria")
                ? workItem.Fields["Microsoft.VSTS.Common.AcceptanceCriteria"]?.ToString() ?? string.Empty
                : string.Empty;

            // Generate the plan based on the work item details
            var plan = new System.Text.StringBuilder();
            plan.AppendLine("Work Item Resolution Plan");
            plan.AppendLine("========================");
            plan.AppendLine($"Work Item #{workItemId}: {workItem.Fields["System.Title"]}");
            plan.AppendLine();

            plan.AppendLine("Requirements Analysis:");
            plan.AppendLine("- Review and understand the user story/requirements");
            plan.AppendLine("- Identify key technical components needed");
            plan.AppendLine("- List any dependencies or constraints");
            plan.AppendLine();

            if (!string.IsNullOrEmpty(acceptanceCriteria))
            {
                plan.AppendLine("Acceptance Criteria:");
                plan.AppendLine(acceptanceCriteria);
                plan.AppendLine();
            }

            plan.AppendLine("Implementation Steps:");
            plan.AppendLine("1. Set up development environment");
            plan.AppendLine("2. Create feature branch");
            plan.AppendLine("3. Implement core functionality");
            plan.AppendLine("4. Write unit tests");
            plan.AppendLine("5. Perform code review");
            plan.AppendLine("6. Update documentation");
            plan.AppendLine();

            plan.AppendLine("Testing Strategy:");
            plan.AppendLine("- Unit tests for all new code");
            plan.AppendLine("- Integration tests for system interactions");
            plan.AppendLine("- Manual testing of UI/UX");
            plan.AppendLine();

            plan.AppendLine("Definition of Done:");
            plan.AppendLine("- All tests passing");
            plan.AppendLine("- Code reviewed and approved");
            plan.AppendLine("- Documentation updated");
            plan.AppendLine("- Acceptance criteria met");

            return plan.ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error generating plan for work item {WorkItemId}", workItemId);
            return $"Error generating plan for work item {workItemId}: {ex.Message}";
        }
    }

    private static string FormatWorkItemDetails(WorkItem workItem)
    {
        var details = new System.Text.StringBuilder();
        details.AppendLine($"Work Item #{workItem.Id}");
        details.AppendLine("==================");

        if (workItem.Fields != null)
        {
            details.AppendLine($"Title: {workItem.Fields["System.Title"]}");
            details.AppendLine($"Type: {workItem.Fields["System.WorkItemType"]}");
            details.AppendLine($"State: {workItem.Fields["System.State"]}");

            if (workItem.Fields.ContainsKey("System.Description"))
            {
                details.AppendLine("\nDescription:");
                details.AppendLine(workItem.Fields["System.Description"]?.ToString());
            }

            if (workItem.Fields.ContainsKey("Microsoft.VSTS.Common.AcceptanceCriteria"))
            {
                details.AppendLine("\nAcceptance Criteria:");
                details.AppendLine(workItem.Fields["Microsoft.VSTS.Common.AcceptanceCriteria"]?.ToString());
            }
        }

        return details.ToString();
    }
}
