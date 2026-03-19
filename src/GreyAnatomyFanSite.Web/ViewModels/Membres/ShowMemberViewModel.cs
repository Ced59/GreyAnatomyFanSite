namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class ShowMemberViewModel
{
    public Guid Id { get; init; }

    public string Pseudo { get; init; } = string.Empty;

    public string AvatarPath { get; init; } = "images/Avatars/Default.jpg";

    public string Status { get; init; } = string.Empty;

    public DateTime RegisteredAtUtc { get; init; }

    public string? Email { get; init; }

    public bool CanSeePrivateInformation { get; init; }

    public bool IsCurrentMember { get; init; }

    public string? SuccessMessage { get; init; }
}
