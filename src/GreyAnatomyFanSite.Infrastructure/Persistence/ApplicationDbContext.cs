using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Domain.Entities;
using GreyAnatomyFanSite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Infrastructure.Persistence;

public sealed class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>,
      IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();

    public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();

    public DbSet<ArticleComment> ArticleComments => Set<ArticleComment>();

    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("app");

        base.OnModelCreating(builder);

        builder.Entity<ArticleCategory>(entity =>
        {
            entity.ToTable("article_categories", "app");
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Title).HasMaxLength(200).IsRequired();
            entity.HasIndex(category => category.Title).IsUnique();
        });

        builder.Entity<MemberProfile>(entity =>
        {
            entity.ToTable("member_profiles", "app");
            entity.HasKey(member => member.Id);
            entity.Property(member => member.Pseudo).HasMaxLength(100).IsRequired();
            entity.Property(member => member.Email).HasMaxLength(320).IsRequired();
            entity.Property(member => member.AvatarPath).HasMaxLength(500);
            entity.Property(member => member.FirstName).HasMaxLength(100);
            entity.Property(member => member.LastName).HasMaxLength(100);
            entity.Property(member => member.Gender).HasMaxLength(50);
            entity.HasIndex(member => member.Pseudo).IsUnique();
            entity.HasIndex(member => member.Email).IsUnique();
        });

        builder.Entity<Article>(entity =>
        {
            entity.ToTable("articles", "app");
            entity.HasKey(article => article.Id);
            entity.Property(article => article.Title).HasMaxLength(250).IsRequired();
            entity.Property(article => article.MediaPath).HasMaxLength(500);
            entity.Property(article => article.MediaType).HasMaxLength(50).IsRequired();
            entity.Property(article => article.Content).HasColumnType("text").IsRequired();

            entity.HasOne(article => article.Category)
                .WithMany(category => category.Articles)
                .HasForeignKey(article => article.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(article => article.AuthorMemberProfile)
                .WithMany(member => member.Articles)
                .HasForeignKey(article => article.AuthorMemberProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ArticleComment>(entity =>
        {
            entity.ToTable("article_comments", "app");
            entity.HasKey(comment => comment.Id);
            entity.Property(comment => comment.Title).HasMaxLength(250).IsRequired();
            entity.Property(comment => comment.Content).HasColumnType("text").IsRequired();

            entity.HasOne(comment => comment.Article)
                .WithMany(article => article.Comments)
                .HasForeignKey(comment => comment.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(comment => comment.AuthorMemberProfile)
                .WithMany(member => member.Comments)
                .HasForeignKey(comment => comment.AuthorMemberProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("users", "identity");
            entity.Property(user => user.Pseudo).HasMaxLength(100).IsRequired();
            entity.Property(user => user.AvatarPath).HasMaxLength(500);
            entity.Property(user => user.FirstName).HasMaxLength(100);
            entity.Property(user => user.LastName).HasMaxLength(100);
            entity.Property(user => user.Gender).HasMaxLength(50);
        });

        builder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("roles", "identity");
        });

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("user_roles", "identity");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("user_claims", "identity");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("user_logins", "identity");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("role_claims", "identity");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("user_tokens", "identity");
        });
    }
}
