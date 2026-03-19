using GreyAnatomyFanSite.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Application.Articles.Queries.GetHomePage;

public sealed record GetHomePageQuery(int Page, int CategoryId) : IRequest<HomePageDto>;

public sealed class GetHomePageQueryHandler : IRequestHandler<GetHomePageQuery, HomePageDto>
{
    private const int PageSize = 10;

    private readonly IApplicationDbContext dbContext;

    public GetHomePageQueryHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<HomePageDto> Handle(GetHomePageQuery request, CancellationToken cancellationToken)
    {
        int currentPage = request.Page <= 0 ? 1 : request.Page;

        IQueryable<GreyAnatomyFanSite.Domain.Entities.Article> baseQuery = dbContext.Articles
            .AsNoTracking()
            .Include(article => article.Category)
            .Include(article => article.Comments)
            .OrderByDescending(article => article.PublishedAtUtc);

        if (request.CategoryId > 0)
        {
            baseQuery = baseQuery.Where(article => article.CategoryId == request.CategoryId);
        }

        int totalCount = await baseQuery.CountAsync(cancellationToken);
        int totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)PageSize);

        List<HomePageArticleDto> articles = await baseQuery
            .Skip((currentPage - 1) * PageSize)
            .Take(PageSize)
            .Select(article => new HomePageArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                MediaPath = article.MediaPath,
                MediaType = article.MediaType,
                PublishedAtUtc = article.PublishedAtUtc,
                CategoryTitle = article.Category.Title,
                CommentCount = article.Comments.Count
            })
            .ToListAsync(cancellationToken);

        List<HomePageCategoryDto> categories = await dbContext.ArticleCategories
            .AsNoTracking()
            .OrderBy(category => category.Title)
            .Select(category => new HomePageCategoryDto
            {
                Id = category.Id,
                Title = category.Title
            })
            .ToListAsync(cancellationToken);

        return new HomePageDto
        {
            ActiveCategoryId = request.CategoryId,
            Articles = articles,
            Categories = categories,
            Birthdays = Array.Empty<HomePageBirthdayDto>(),
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }
}
