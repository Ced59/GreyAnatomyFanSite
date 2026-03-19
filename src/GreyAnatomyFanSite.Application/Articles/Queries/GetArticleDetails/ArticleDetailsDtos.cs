namespace GreyAnatomyFanSite.Application.Articles.Queries.GetArticleDetails
{
    public sealed class ArticleDetailsDto
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public string? MediaPath { get; init; }

        public string MediaType { get; init; } = string.Empty;

        public DateTime PublishedAtUtc { get; init; }

        public string CategoryTitle { get; init; } = string.Empty;

        public string AuthorPseudo { get; init; } = string.Empty;

        public IReadOnlyCollection<ArticleCommentDto> Comments { get; init; } = Array.Empty<ArticleCommentDto>();

        public IReadOnlyCollection<RelatedArticleDto> LatestArticles { get; init; } = Array.Empty<RelatedArticleDto>();
    }

    public sealed class ArticleCommentDto
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public DateTime CreatedAtUtc { get; init; }

        public string AuthorPseudo { get; init; } = string.Empty;

        public string? AuthorAvatarPath { get; init; }
    }

    public sealed class RelatedArticleDto
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public string? MediaPath { get; init; }

        public string MediaType { get; init; } = string.Empty;
    }
}
