using Github.Api.ViewModels;

namespace Github.Api.Services;

/// <summary>
/// Github Service
/// </summary>
/// <param name="logger"></param>
/// <param name="httpClient"></param>
public class GithubService(ILogger<GithubService> logger, HttpClient httpClient)
{
    /// <summary>
    /// Fetches the user's git profile based on username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The <see cref="GithubUser"/> if found otherwise null</returns>
    /// <exception cref="ApplicationException"></exception>
    public async Task<GithubUser?> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request for {ServiceName} with request data: {Username}", 
            nameof(GetUserAsync),
            username);
        
        // 70% of time it will thrown an exception for demo purpose of Polly
        if (Random.Shared.NextDouble() < 0.7)
        {
            logger.LogError("The Github API is unavailable.");
            throw new ApplicationException("The Github API is unavailable.");
        }
        var user = await httpClient
            .GetFromJsonAsync<GithubUser>($"users/{username}", cancellationToken);
        
        return user;
    }
}