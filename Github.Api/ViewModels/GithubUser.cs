using System.Text.Json.Serialization;

namespace Github.Api.ViewModels;

/// <summary>
/// Github User
/// </summary>
public class GithubUser
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
    public string? Url { get; set; }

    public int Followers { get; set; }
    public int Following { get; set; }
    
    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public static GithubUser Blank => new GithubUser
    {
        Id = -1,
        Name = string.Empty,
        Email = string.Empty,
        Bio = string.Empty,
        Url = string.Empty,
        Followers = -1,
        Following = -1,
        AvatarUrl = string.Empty,
        CreatedAt = default,
        UpdatedAt = default
    };
}