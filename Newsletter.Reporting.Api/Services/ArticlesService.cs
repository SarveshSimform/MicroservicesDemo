using ErrorOr;
using Newsletter.Reporting.Api.Errors;
using Newsletter.Reporting.Api.Repositories;
using Newsletter.Reporting.Api.ViewModels;

namespace Newsletter.Reporting.Api.Services;

public class ArticlesService(IArticlesRepository articlesRepository, ILogger<ArticlesService> logger) : IArticlesService
{
    public async Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request for service: {ServiceName} with request data: {RequestData}", 
            nameof(GetArticleById),
            id);
        
        var articleWithEvents = await articlesRepository.GetArticleWithEventsByIdAsync(id, cancellationToken);
        if (articleWithEvents is null)
        {
            return ArticlesErrors.ArticleNotFound;
        }
        return articleWithEvents;
    }
}