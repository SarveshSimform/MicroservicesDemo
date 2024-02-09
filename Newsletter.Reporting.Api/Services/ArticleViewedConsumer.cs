using MassTransit;
using Newsletter.Reporting.Api.Entities;
using Newsletter.Reporting.Api.Repositories;
using Newsletter.Shared;

namespace Newsletter.Reporting.Api.Services;

public class ArticleViewedConsumer(IArticlesRepository articlesRepository, IArticleEventsRepository articleEventsRepository, ILogger<ArticleViewedConsumer> logger) : IConsumer<ArticleViewedEvent>
{
    public async Task Consume(ConsumeContext<ArticleViewedEvent> context)
    {
        // Access the correlation ID from the message headers
        var correlationId = context.Headers.Get<string>("correlationId");
        Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);
        
        logger.LogInformation("Received request for ArticleViewedConsumer with request data: {RequestData}", 
            context.Message);
        
        var article = await articlesRepository.GetArticleByIdAsync(context.Message.Id, context.CancellationToken);
        if (article is null)
        {
            return;
        }

        var articleEvent = new ArticleEvent
        {
            Id = Guid.NewGuid(),
            ArticleId = article.Id,
            CreatedOnUtc = context.Message.ViewedOnUtc,
            EventType = ArticleEventType.ArticleViewed
        };
        
        articleEventsRepository.Add(articleEvent);
        
        // CancellationToken.None because we don't want to cancel adding of an event coming from message bus
        await articleEventsRepository.SaveChangesAsync(CancellationToken.None);
        
        logger.LogInformation("Created a new article view event in ArticleViewedConsumer with data: {ArticleEventData}", 
            articleEvent);
    }
}