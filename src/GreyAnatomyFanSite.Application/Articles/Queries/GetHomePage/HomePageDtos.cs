namespace GreyAnatomyFanSite.Application.Articles.Queries.GetHomePage
{
    public sealed class HomePageDto
    {
        public IReadOnlyCollection<HomePageBirthdayDto> Birthdays { get; init; } = Array.Empty<HomePageBirthdayDto>();

        public IReadOnlyCollection<HomePageArticleDto> Articles { get; init; } = Array.Empty<HomePageArticleDto>();

        public IReadOnlyCollection<HomePageCategoryDto> Categories { get; init; } = Array.Empty<HomePageCategoryDto>();

        public int CurrentPage { get; init; }

        public int TotalPages { get; init; }

        public int ActiveCategoryId { get; init; }
    }

    public sealed class HomePageBirthdayDto
    {
        public string ActorFirstName { get; init; } = string.Empty;

        public string ActorLastName { get; init; } = string.Empty;

        public string CharacterFirstName { get; init; } = string.Empty;

        public string CharacterLastName { get; init; } = string.Empty;

        public DateTime DateOfBirth { get; init; }
    }

    public sealed class HomePageArticleDto
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public string? MediaPath { get; init; }

        public string MediaType { get; init; } = string.Empty;

        public DateTime PublishedAtUtc { get; init; }

        public string CategoryTitle { get; init; } = string.Empty;

        public int CommentCount { get; init; }
    }

    public sealed class HomePageCategoryDto
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;
    }
}
