using AutoMapper;
using ErrorOr;
using MassTransit;
using Newsletter.Api.Entities;
using Newsletter.Api.Repositories;
using Newsletter.Api.ViewModels;
using Newsletter.Api.Errors;
using Newsletter.Shared;

namespace Newsletter.Api.Services;

public class ArticlesService(
    IArticlesRepository articlesRepository, 
    ILogger<ArticlesService> logger, 
    IMapper mapper, 
    IPublishEndpoint publishEndpoint,
    IHttpContextAccessor httpContextAccessor) : IArticlesService
{
    public async Task<ErrorOr<ArticleResponse>> CreateArticleAsync(CreateArticle createArticle, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request for service: {ServiceName} with request data: {RequestData}", 
            nameof(CreateArticleAsync),
            createArticle);
        
        var article = mapper.Map<Article>(createArticle);
        
        articlesRepository.CreateArticle(article, cancellationToken);
        await articlesRepository.SaveChangesAsync(cancellationToken);

        var articleCreatedEvent = new ArticleCreatedEvent
        {
            Id = article.Id,
            CreatedOnUtc = article.CreatedOnUtc
        };
        
        logger.LogInformation("Sending ArticleCreatedEvent to message queue at {DateTime}", DateTime.UtcNow);

        var correlationId = Guid.TryParse(httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString(), out var result) ? result : Guid.Empty;
        await publishEndpoint.Publish(articleCreatedEvent, context =>
        {
            context.Headers.Set("correlationId", correlationId);
        }, cancellationToken);
        
        return mapper.Map<ArticleResponse>(article);;
    }

    public async Task<List<ArticleResponse>> GetArticlesAsync()
    {
        logger.LogInformation("Received request for service: {ServiceName}", 
            nameof(GetArticlesAsync));
        
        return mapper.Map<List<ArticleResponse>>(await articlesRepository.GetArticlesAsync());
    }

    public async Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request for service: {ServiceName} with request data: {RequestData}", 
            nameof(GetArticleById),
            id);
        
        var article = await articlesRepository.GetArticleByIdAsync(id, cancellationToken);
        if (article is null)
        {
            return ArticlesErrors.ArticleNotFound;
        }

        var articleViewedEvent = new ArticleViewedEvent
        {
            Id = article.Id,
            ViewedOnUtc = DateTime.UtcNow
        };
        
        logger.LogInformation("Sending ArticleViewedEvent to message queue at {DateTime}", DateTime.UtcNow);

        var correlationId = Guid.TryParse(httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString(), out var result) ? result : Guid.Empty;
        await publishEndpoint.Publish(articleViewedEvent, context =>
        {
            context.Headers.Set("correlationId", correlationId);
        },cancellationToken);
        
        return mapper.Map<ArticleResponse>(article);
    }
}