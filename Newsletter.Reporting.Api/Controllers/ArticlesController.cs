using Microsoft.AspNetCore.Mvc;
using Newsletter.Reporting.Api.Services;

namespace Newsletter.Reporting.Api.Controllers;

[Route("api/[controller]")]
public class ArticlesController(IArticlesService articlesService) : BaseController
{
    [HttpGet("{id:guid}", Name = nameof(GetArticleById))]
    public async Task<ActionResult> GetArticleById(Guid id, CancellationToken cancellationToken)
    {
        var articleResult = await articlesService.GetArticleById(id, cancellationToken);
        return articleResult.Match(
            Ok,
            Problem);
    }
}