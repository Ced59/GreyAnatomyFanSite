namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class LoginPageViewModel
{
    public string? Mail { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

    public string? SuccessMessage { get; init; }

    public string? TypePubli { get; init; }

    public int? IdPubli { get; init; }
}
