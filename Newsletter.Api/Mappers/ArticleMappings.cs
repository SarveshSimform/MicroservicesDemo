using AutoMapper;
using Newsletter.Api.Entities;
using Newsletter.Api.ViewModels;

namespace Newsletter.Api.Mappers;

public class ArticleMappings : Profile
{
    public ArticleMappings()
    {
        CreateMap<CreateArticle, Article>();
        CreateMap<Article, ArticleResponse>();
    }
}