namespace GreyAnatomyFanSite.Application.Common.Models;

public sealed class MemberProfileDetails
{
    public Guid Id { get; init; }

    public string Pseudo { get; init; } = string.Empty;

    public string? Email { get; init; }

    public string? AvatarPath { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime RegisteredAtUtc { get; init; }

    public bool IsActive { get; init; }
}
