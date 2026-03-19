using Microsoft.AspNetCore.Identity;

namespace GreyAnatomyFanSite.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string Pseudo { get; set; } = string.Empty;

    public string? AvatarPath { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime RegisteredAtUtc { get; set; }

    public bool IsActive { get; set; } = true;
}
