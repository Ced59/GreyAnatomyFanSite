namespace GreyAnatomyFanSite.Web.ViewModels.Membres;

public sealed class ConfirmMemberViewModel
{
    public bool Succeeded { get; init; }

    public string? Pseudo { get; init; }

    public string Message { get; init; } = string.Empty;
}
