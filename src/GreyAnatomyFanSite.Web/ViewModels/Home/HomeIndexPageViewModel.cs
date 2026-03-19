namespace GreyAnatomyFanSite.Web.ViewModels.Home
{
    public sealed class HomeIndexPageViewModel
    {
        public IReadOnlyCollection<BirthdayActorViewModel> BirthDatesActeurs { get; init; } = Array.Empty<BirthdayActorViewModel>();

        public IReadOnlyCollection<HomeArticleSummaryViewModel> Articles { get; init; } = Array.Empty<HomeArticleSummaryViewModel>();

        public int NbrePagePagination { get; init; }

        public int PagePagination { get; init; }

        public IReadOnlyCollection<ArticleCategoryOptionViewModel> CategoryArticles { get; init; } = Array.Empty<ArticleCategoryOptionViewModel>();

        public int ActiveCategory { get; init; }
    }

    public sealed class BirthdayActorViewModel
    {
        public string ActorFirstName { get; init; } = string.Empty;

        public string ActorLastName { get; init; } = string.Empty;

        public string CharacterFirstName { get; init; } = string.Empty;

        public string CharacterLastName { get; init; } = string.Empty;

        public DateTime DateNaissance { get; init; }
    }

    public sealed class HomeArticleSummaryViewModel
    {
        public int Id { get; init; }

        public string Titre { get; init; } = string.Empty;

        public string Texte { get; init; } = string.Empty;

        public string? Media { get; init; }

        public string TypeMedia { get; init; } = string.Empty;

        public DateTime Date { get; init; }

        public ArticleCategoryOptionViewModel Categorie { get; init; } = null!;

        public int CommentairesCount { get; init; }
    }

    public sealed class ArticleCategoryOptionViewModel
    {
        public int Id { get; init; }

        public string TitreCategory { get; init; } = string.Empty;
    }
}
