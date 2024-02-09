using Newsletter.Api.Data;
using Newsletter.Api.Entities;

namespace Newsletter.Api.Repositories;

public class ArticlesRepository(ApplicationDbContext context) : IArticlesRepository
{
    public Task<List<Article>> GetArticlesAsync()
    {
        return Task.FromResult(context.Articles.ToList());
    }

    public async Task<Article?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Articles.FindAsync([id], cancellationToken: cancellationToken);
    }

    public void CreateArticle(Article article, CancellationToken cancellationToken)
    {
        context.Articles.Add(article);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}