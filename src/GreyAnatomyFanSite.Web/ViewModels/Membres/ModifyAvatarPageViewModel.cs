namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class ModifyAvatarPageViewModel
{
    public string AvatarPath { get; init; } = "images/Avatars/Default.jpg";

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
