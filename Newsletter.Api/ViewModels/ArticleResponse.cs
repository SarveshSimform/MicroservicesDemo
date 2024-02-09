namespace Newsletter.Api.ViewModels;

public record ArticleResponse(string Id, string Title, string Content, DateTime CreatedOnUtc);