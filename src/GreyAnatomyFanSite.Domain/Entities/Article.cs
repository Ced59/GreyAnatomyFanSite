using GreyAnatomyFanSite.Domain.Common;

namespace GreyAnatomyFanSite.Domain.Entities
{
    public sealed class Article : BaseAuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string? MediaPath { get; set; }

        public string MediaType { get; set; } = "image";

        public DateTime PublishedAtUtc { get; set; }

        public int CategoryId { get; set; }

        public ArticleCategory Category { get; set; } = null!;

        public Guid AuthorMemberProfileId { get; set; }

        public MemberProfile AuthorMemberProfile { get; set; } = null!;

        public ICollection<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
    }
}
