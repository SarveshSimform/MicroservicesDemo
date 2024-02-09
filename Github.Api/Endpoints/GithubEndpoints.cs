using Github.Api.Services;
using Github.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;

namespace Github.Api.Endpoints;

public static class GithubEndpoints
{
    /// <summary>
    /// Github Endpoints
    /// </summary>
    /// <param name="routeBuilder"></param>
    public static void MapGithubEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        // Retry policy of Polly
        routeBuilder.MapGet("api/retry-policy/users/{username}", async (
            string username,
            GithubService githubService,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
        {
            
            // Polly resilience pipeline (Retry)
            var resiliencePipeline =  new ResiliencePipelineBuilder<GithubUser?>()
                .AddRetry(new RetryStrategyOptions<GithubUser?>
                {
                    MaxRetryAttempts = 2,
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.Zero,
                    ShouldHandle = new PredicateBuilder<GithubUser?>()
                        .Handle<ApplicationException>(),
                    OnRetry = arguments =>
                    {
                        logger.LogInformation("Polly retry attempt count: {Count} with exception: {Exception}", 
                            arguments.AttemptNumber, 
                            arguments.Outcome.Exception);
                        return ValueTask.CompletedTask;
                    }
                }).Build();

            var githubUser =  await resiliencePipeline.ExecuteAsync(
                async token => await githubService.GetUserAsync(username, token), cancellationToken);
            
            return Results.Ok(githubUser);
        });
        
        // Retry and wait policy of Polly
        routeBuilder.MapGet("api/retry-and-wait-policy/users/{username}", async (
            string username,
            GithubService githubService,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
        {
            
            // Polly resilience pipeline (Retry)
            var resiliencePipeline =  new ResiliencePipelineBuilder<GithubUser?>()
                .AddRetry(new RetryStrategyOptions<GithubUser?>
                {
                    MaxRetryAttempts = 5,
                    DelayGenerator = static args =>
                    {
                        var delay = args.AttemptNumber * TimeSpan.FromSeconds(2);
                        return new ValueTask<TimeSpan?>(delay);
                    },
                    ShouldHandle = new PredicateBuilder<GithubUser?>()
                        .Handle<ApplicationException>(),
                    OnRetry = arguments =>
                    {
                        logger.LogInformation("Polly retry attempt count: {Count} with exception: {Exception}", 
                            arguments.AttemptNumber, 
                            arguments.Outcome.Exception);
                        return ValueTask.CompletedTask;
                    }
                }).Build();

            var githubUser =  await resiliencePipeline.ExecuteAsync(
                async token => await githubService.GetUserAsync(username, token), cancellationToken);
            
            return Results.Ok(githubUser);
        });
        
        // Fallback policy of Polly
        routeBuilder.MapGet("api/fallback-policy/users/{username}", async (
            string username,
            GithubService githubService,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
        {
            
            // Polly resilience pipeline (Retry)
            var resiliencePipeline =  new ResiliencePipelineBuilder<GithubUser?>()
                .AddFallback(new FallbackStrategyOptions<GithubUser?>()
                {
                    FallbackAction = _ => Outcome.FromResultAsValueTask<GithubUser?>(GithubUser.Blank)
                }).Build();

            var githubUser =  await resiliencePipeline.ExecuteAsync(
                async token => await githubService.GetUserAsync(username, token), cancellationToken);
            
            return Results.Ok(githubUser);
        });
        
        // Circuit breaker policy of Polly
        routeBuilder.MapGet("api/circuit-breaker-policy/users/{username}", async (
            string username,
            GithubService githubService,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
        {
            
            // Polly resilience pipeline (Retry)
            var resiliencePipeline =  new ResiliencePipelineBuilder<GithubUser?>()
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions<GithubUser?>
                {
                    FailureRatio = 0.7,
                    BreakDuration = TimeSpan.FromSeconds(30),
                    ShouldHandle = new PredicateBuilder<GithubUser?>()
                        .Handle<Exception>(),
                    OnOpened = arguments =>
                    {
                        logger.LogInformation("Circuit is in open state");
                        return ValueTask.CompletedTask;
                    },
                    OnHalfOpened = arguments =>
                    {
                        logger.LogInformation("Circuit is in half-open state");
                        return ValueTask.CompletedTask;
                    }
                }).Build();

            var githubUser =  await resiliencePipeline.ExecuteAsync(
                async token => await githubService.GetUserAsync(username, token), cancellationToken);
            
            return Results.Ok(githubUser);
        });
    }
}