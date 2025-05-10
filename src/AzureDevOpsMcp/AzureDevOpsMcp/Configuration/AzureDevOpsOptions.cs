namespace AzureDevOpsMcp.Configuration;

public class AzureDevOpsOptions
{
    public const string SectionName = "AzureDevOps";

    public string OrganizationUrl { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string TokenEnvVar { get; set; } = "AZURE_DEVOPS_TOKEN";
}
