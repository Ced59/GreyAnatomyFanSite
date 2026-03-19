using GreyAnatomyFanSite.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Application.Articles.Queries.GetArticleDetails;

public sealed record GetArticleDetailsQuery(int ArticleId) : IRequest<ArticleDetailsDto?>;

public sealed class GetArticleDetailsQueryHandler : IRequestHandler<GetArticleDetailsQuery, ArticleDetailsDto?>
{
    private readonly IApplicationDbContext dbContext;

    public GetArticleDetailsQueryHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ArticleDetailsDto?> Handle(GetArticleDetailsQuery request, CancellationToken cancellationToken)
    {
        ArticleDetailsDto? article = await dbContext.Articles
            .AsNoTracking()
            .Include(entity => entity.Category)
            .Include(entity => entity.AuthorMemberProfile)
            .Include(entity => entity.Comments)
                .ThenInclude(entity => entity.AuthorMemberProfile)
            .Where(entity => entity.Id == request.ArticleId)
            .Select(entity => new ArticleDetailsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                MediaPath = entity.MediaPath,
                MediaType = entity.MediaType,
                PublishedAtUtc = entity.PublishedAtUtc,
                CategoryTitle = entity.Category.Title,
                AuthorPseudo = entity.AuthorMemberProfile.Pseudo,
                Comments = entity.Comments
                    .OrderByDescending(comment => comment.CreatedAtUtc)
                    .Select(comment => new ArticleCommentDto
                    {
                        Id = comment.Id,
                        Title = comment.Title,
                        Content = comment.Content,
                        CreatedAtUtc = comment.CreatedAtUtc,
                        AuthorPseudo = comment.AuthorMemberProfile.Pseudo,
                        AuthorAvatarPath = comment.AuthorMemberProfile.AvatarPath
                    })
                    .ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (article is null)
        {
            return null;
        }

        List<RelatedArticleDto> latestArticles = await dbContext.Articles
            .AsNoTracking()
            .Where(entity => entity.Id != request.ArticleId)
            .OrderByDescending(entity => entity.PublishedAtUtc)
            .Take(10)
            .Select(entity => new RelatedArticleDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                MediaPath = entity.MediaPath,
                MediaType = entity.MediaType
            })
            .ToListAsync(cancellationToken);

        return new ArticleDetailsDto
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            MediaPath = article.MediaPath,
            MediaType = article.MediaType,
            PublishedAtUtc = article.PublishedAtUtc,
            CategoryTitle = article.CategoryTitle,
            AuthorPseudo = article.AuthorPseudo,
            Comments = article.Comments,
            LatestArticles = latestArticles
        };
    }
}
