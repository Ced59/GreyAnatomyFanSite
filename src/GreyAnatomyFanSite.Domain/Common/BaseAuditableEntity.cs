namespace GreyAnatomyFanSite.Domain.Common;

public abstract class BaseAuditableEntity
{
    public DateTime CreatedAtUtc { get; set; }

    public DateTime? LastModifiedAtUtc { get; set; }
}
