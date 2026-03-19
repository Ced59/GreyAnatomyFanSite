namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class ResetPasswordPageViewModel
{
    public Guid UserId { get; init; }

    public string Token { get; init; } = string.Empty;

    public string Pseudo { get; init; } = string.Empty;

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
