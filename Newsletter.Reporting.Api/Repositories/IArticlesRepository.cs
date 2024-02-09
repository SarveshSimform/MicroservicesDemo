using Newsletter.Reporting.Api.Entities;
using Newsletter.Reporting.Api.ViewModels;

namespace Newsletter.Reporting.Api.Repositories;

public interface IArticlesRepository
{
    Task<List<Article>> GetArticlesAsync();
    Task<Article?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ArticleResponse?> GetArticleWithEventsByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Article article);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}