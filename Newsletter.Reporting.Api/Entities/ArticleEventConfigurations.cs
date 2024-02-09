using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Newsletter.Reporting.Api.Entities;

public class ArticleEventConfigurations : IEntityTypeConfiguration<ArticleEvent>
{
    public void Configure(EntityTypeBuilder<ArticleEvent> builder)
    {
        builder.HasKey(x => x.Id);
    }
}