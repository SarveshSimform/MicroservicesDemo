using ErrorOr;
using Newsletter.Api.ViewModels;

namespace Newsletter.Api.Services;

public interface IArticlesService
{
    Task<ErrorOr<ArticleResponse>> CreateArticleAsync(CreateArticle createArticle, CancellationToken cancellationToken);
    Task<List<ArticleResponse>> GetArticlesAsync();
    Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken);
}