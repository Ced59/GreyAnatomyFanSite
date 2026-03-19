using GreyAnatomyFanSite.Domain.Common;

namespace GreyAnatomyFanSite.Domain.Entities;

public sealed class ArticleCategory : BaseAuditableEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
