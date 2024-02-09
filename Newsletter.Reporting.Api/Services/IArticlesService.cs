using ErrorOr;
using Newsletter.Reporting.Api.ViewModels;

namespace Newsletter.Reporting.Api.Services;

public interface IArticlesService
{
    Task<ErrorOr<ArticleResponse>> GetArticleById(Guid id, CancellationToken cancellationToken);
}