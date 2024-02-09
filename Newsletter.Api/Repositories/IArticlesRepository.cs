using Newsletter.Api.Entities;

namespace Newsletter.Api.Repositories;

public interface IArticlesRepository
{
    Task<List<Article>> GetArticlesAsync();
    Task<Article?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
    void CreateArticle(Article article, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}