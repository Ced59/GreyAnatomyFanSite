using GreyAnatomyFanSite.Domain.Common;

namespace GreyAnatomyFanSite.Domain.Entities
{
    public sealed class ArticleComment : BaseAuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int ArticleId { get; set; }

        public Article Article { get; set; } = null!;

        public Guid AuthorMemberProfileId { get; set; }

        public MemberProfile AuthorMemberProfile { get; set; } = null!;
    }
}
