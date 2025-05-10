using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Moq;
using AzureDevOpsMcp.Tools;

namespace AzureDevOpsMcp.Tests;

public class WorkItemToolsTests
{
    private readonly Mock<WorkItemTrackingHttpClient> _mockClient;

    public WorkItemToolsTests()
    {
        _mockClient = new Mock<WorkItemTrackingHttpClient>(MockBehavior.Strict);
    }

    [Fact]
    public async Task GetWorkItemDescription_WhenWorkItemExists_ReturnsFormattedDetails()
    {
        // Arrange
        var workItemId = 123;
        var workItem = new WorkItem
        {
            Id = workItemId,
            Fields = new Dictionary<string, object>
            {
                { "System.Title", "Test Work Item" },
                { "System.Description", "Test Description" },
                { "System.State", "Active" },
                { "System.WorkItemType", "User Story" }
            }
        };

        _mockClient
            .Setup(x => x.GetWorkItemAsync(workItemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(workItem);

        // Act
        var result = await WorkItemTools.GetWorkItemDescription(_mockClient.Object, workItemId);

        // Assert
        Assert.Contains("Title: Test Work Item", result);
        Assert.Contains("Type: User Story", result);
        Assert.Contains("State: Active", result);
        Assert.Contains("Description: Test Description", result);
        _mockClient.VerifyAll();
    }

    [Fact]
    public async Task GetWorkItemDescription_WhenWorkItemNotFound_ReturnsNotFoundMessage()
    {
        // Arrange
        var workItemId = 456;
        _mockClient
            .Setup(x => x.GetWorkItemAsync(workItemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkItem)null);

        // Act
        var result = await WorkItemTools.GetWorkItemDescription(_mockClient.Object, workItemId);

        // Assert
        Assert.Equal($"Work item {workItemId} not found.", result);
        _mockClient.VerifyAll();
    }

    [Fact]
    public async Task GeneratePlanForWorkItem_WhenWorkItemExists_ReturnsImplementationPlan()
    {
        // Arrange
        var workItemId = 789;
        var workItem = new WorkItem
        {
            Id = workItemId,
            Fields = new Dictionary<string, object>
            {
                { "System.Title", "Implement Feature X" },
                { "System.Description", "Need to implement feature X with following requirements..." },
                { "Microsoft.VSTS.Common.AcceptanceCriteria", "1. Should do A\n2. Should handle B\n3. Must be secure" }
            }
        };

        _mockClient
            .Setup(x => x.GetWorkItemAsync(workItemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(workItem);

        // Act
        var result = await WorkItemTools.GeneratePlanForWorkItem(_mockClient.Object, workItemId);

        // Assert
        Assert.Contains("Implementation Plan for", result);
        Assert.Contains("Requirements:", result);
        Assert.Contains("Acceptance Criteria:", result);
        _mockClient.VerifyAll();
    }
}
