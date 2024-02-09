namespace Github.Api.Configurations;

/// <summary>
/// Github Settings
/// </summary>
public class GithubSettings
{
    public const string Key = "GithubSettings";
    public required string AccessToken { get; init; }
    public required string UserAgent { get; init; }
    public required string BaseAddress { get; init; }
}