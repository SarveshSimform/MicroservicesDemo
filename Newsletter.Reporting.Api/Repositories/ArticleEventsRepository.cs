using Newsletter.Reporting.Api.Data;
using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Repositories;

public class ArticleEventsRepository(ApplicationDbContext context) : IArticleEventsRepository
{
    public Task<List<ArticleEvent>> GetArticleEventsByArticleId(Guid id)
    {
        return Task.FromResult(context.ArticleEvents.Where(articleEvent => articleEvent.ArticleId == id).ToList());
    }

    public void Add(ArticleEvent articleEvent)
    {
        context.ArticleEvents.Add(articleEvent);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}