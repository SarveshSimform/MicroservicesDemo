namespace Newsletter.Shared;

public record ArticleCreatedEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}