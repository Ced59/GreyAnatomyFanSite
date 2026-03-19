namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class RegisterPageViewModel
{
    public string? Pseudo { get; init; }

    public string? Mail { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
