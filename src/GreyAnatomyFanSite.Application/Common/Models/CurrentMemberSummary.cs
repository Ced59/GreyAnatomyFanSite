namespace GreyAnatomyFanSite.Application.Common.Models;

public sealed class CurrentMemberSummary
{
    public Guid Id { get; init; }

    public string Pseudo { get; init; } = string.Empty;

    public string? AvatarPath { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
}
