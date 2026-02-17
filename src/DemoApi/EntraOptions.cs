namespace DemoApi;

public sealed class EntraOptions
{
    public const string SectionName = "Entra";

    public string TenantId { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Instance { get; init; } = "https://login.microsoftonline.com/";
}
