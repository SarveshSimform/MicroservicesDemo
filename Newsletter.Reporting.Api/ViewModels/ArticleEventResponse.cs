using Newsletter.Reporting.Api.Entities;

namespace Newsletter.Reporting.Api.ViewModels;

public record ArticleEventResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public ArticleEventType EventType { get; init; }
}