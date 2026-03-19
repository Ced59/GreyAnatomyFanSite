namespace GreyAnatomyFanSite.Web.ViewModels.Comments;

public sealed class ArticleCommentViewModel
{
    public string Titre { get; init; } = string.Empty;

    public string Text { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public string AuteurPseudo { get; init; } = string.Empty;

    public string AuteurAvatar { get; init; } = "images/Avatars/Default.jpg";
}
