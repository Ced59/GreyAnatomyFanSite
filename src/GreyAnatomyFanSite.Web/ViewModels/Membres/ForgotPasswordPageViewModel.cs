namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class ForgotPasswordPageViewModel
{
    public string? Mail { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

    public string? SuccessMessage { get; init; }

    public string? DebugActionUrl { get; init; }

    public string? DebugActionLabel { get; init; }
}
