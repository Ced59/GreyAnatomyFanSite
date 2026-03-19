namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class RegistrationMailSentViewModel
{
    public string Mail { get; init; } = string.Empty;

    public string? SuccessMessage { get; init; }

    public string? DebugActionUrl { get; init; }

    public string? DebugActionLabel { get; init; }
}
