using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.Repositories;

public interface IArticleEventsRepository
{
    Task<List<ArticleEvent>> GetArticleEventsByArticleId(Guid id);
    void Add(ArticleEvent articleEvent);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}