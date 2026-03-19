using GreyAnatomyFanSite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Article> Articles { get; }

        DbSet<ArticleCategory> ArticleCategories { get; }

        DbSet<ArticleComment> ArticleComments { get; }

        DbSet<MemberProfile> MemberProfiles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
