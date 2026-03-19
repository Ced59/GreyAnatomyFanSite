using GreyAnatomyFanSite.Domain.Common;

namespace GreyAnatomyFanSite.Domain.Entities;

public sealed class MemberProfile : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Pseudo { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? AvatarPath { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Article> Articles { get; set; } = new List<Article>();

    public ICollection<ArticleComment> Comments { get; set; } = new List<ArticleComment>();
}
