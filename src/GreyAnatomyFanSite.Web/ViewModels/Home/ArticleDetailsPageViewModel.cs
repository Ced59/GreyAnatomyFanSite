using GreyAnatomyFanSite.Web.ViewModels.Comments;

namespace GreyAnatomyFanSite.Web.ViewModels.Home
{
    public sealed class ArticleDetailsPageViewModel
    {
        public ArticleViewModel Article { get; init; } = null!;

        public IReadOnlyCollection<ArticleCommentViewModel> Commentaires { get; init; } = Array.Empty<ArticleCommentViewModel>();

        public IReadOnlyCollection<RelatedArticleViewModel> Articles { get; init; } = Array.Empty<RelatedArticleViewModel>();

        public bool IsLogged { get; init; }

        public string? LoginEmail { get; init; }

        public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
    }

    public sealed class ArticleViewModel
    {
        public int Id { get; init; }

        public string Titre { get; init; } = string.Empty;

        public string Texte { get; init; } = string.Empty;

        public string? Media { get; init; }

        public string TypeMedia { get; init; } = string.Empty;

        public DateTime Date { get; init; }

        public string AuteurPseudo { get; init; } = string.Empty;

        public string CategoryTitle { get; init; } = string.Empty;
    }

    public sealed class RelatedArticleViewModel
    {
        public int Id { get; init; }

        public string Titre { get; init; } = string.Empty;

        public string Texte { get; init; } = string.Empty;

        public string? Media { get; init; }

        public string TypeMedia { get; init; } = string.Empty;
    }
}
