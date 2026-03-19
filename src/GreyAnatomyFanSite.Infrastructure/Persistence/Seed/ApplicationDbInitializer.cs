using GreyAnatomyFanSite.Domain.Constants;
using GreyAnatomyFanSite.Domain.Entities;
using GreyAnatomyFanSite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreyAnatomyFanSite.Infrastructure.Persistence.Seed
{
    public sealed class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<ApplicationDbInitializer> logger;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationDbInitializer(
            ApplicationDbContext dbContext,
            ILogger<ApplicationDbInitializer> logger,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);

            foreach (string roleName in ApplicationRoles.All)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    IdentityRole<Guid> role = new IdentityRole<Guid>(roleName);
                    await roleManager.CreateAsync(role);
                }
            }

            await SeedUsersAsync();
            await SeedContentAsync(cancellationToken);
        }

        private async Task SeedUsersAsync()
        {
            string adminEmail = "admin@greys.local";
            ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    Pseudo = "AdminGreys",
                    AvatarPath = "images/Avatars/Default.jpg",
                    RegisteredAtUtc = DateTime.UtcNow,
                    IsActive = true
                };

                IdentityResult createResult = await userManager.CreateAsync(adminUser, "Admin123!");

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException("Impossible de créer le compte administrateur de démonstration.");
                }

                await userManager.AddToRolesAsync(adminUser, new[] { ApplicationRoles.Administrator, ApplicationRoles.Member });

                dbContext.MemberProfiles.Add(new MemberProfile
                {
                    Id = adminUser.Id,
                    Pseudo = adminUser.Pseudo,
                    Email = adminEmail,
                    AvatarPath = adminUser.AvatarPath,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true
                });
            }

            string heartEmail = "coeur@greys.local";
            ApplicationUser? heartUser = await userManager.FindByEmailAsync(heartEmail);

            if (heartUser is null)
            {
                heartUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = heartEmail,
                    Email = heartEmail,
                    Pseudo = "MonCoeur",
                    AvatarPath = "images/Avatars/Default.jpg",
                    RegisteredAtUtc = DateTime.UtcNow,
                    IsActive = true
                };

                IdentityResult createResult = await userManager.CreateAsync(heartUser, "Coeur123!");

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException("Impossible de créer le compte coeur de démonstration.");
                }

                await userManager.AddToRolesAsync(heartUser, new[] { ApplicationRoles.Heart, ApplicationRoles.Member });

                dbContext.MemberProfiles.Add(new MemberProfile
                {
                    Id = heartUser.Id,
                    Pseudo = heartUser.Pseudo,
                    Email = heartEmail,
                    AvatarPath = heartUser.AvatarPath,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true
                });
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SeedContentAsync(CancellationToken cancellationToken)
        {
            if (await dbContext.ArticleCategories.AnyAsync(cancellationToken))
            {
                return;
            }

            MemberProfile author = await dbContext.MemberProfiles
                .OrderBy(member => member.CreatedAtUtc)
                .FirstAsync(cancellationToken);

            ArticleCategory actualites = new ArticleCategory
            {
                Title = "Actualités",
                CreatedAtUtc = DateTime.UtcNow
            };

            ArticleCategory coulisses = new ArticleCategory
            {
                Title = "Coulisses",
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.ArticleCategories.AddRange(actualites, coulisses);
            await dbContext.SaveChangesAsync(cancellationToken);

            Article article = new Article
            {
                Title = "Bienvenue sur la refonte .NET 10",
                Content = "Ce premier article sert de donnée de démonstration pour la nouvelle architecture.\n\nLe design legacy est conservé, mais le socle technique repart sur .NET 10, EF Core, PostgreSQL, Identity et MediatR.",
                MediaPath = "images/SiteImg/bannieregreysanatomy.jpg",
                MediaType = "image",
                PublishedAtUtc = DateTime.UtcNow,
                CategoryId = actualites.Id,
                AuthorMemberProfileId = author.Id,
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.Articles.Add(article);
            await dbContext.SaveChangesAsync(cancellationToken);

            ArticleComment comment = new ArticleComment
            {
                ArticleId = article.Id,
                AuthorMemberProfileId = author.Id,
                Title = "Premier commentaire",
                Content = "La base Clean Architecture est prête. La suite consiste à migrer progressivement les autres modules sans casser l'UI.",
                CreatedAtUtc = DateTime.UtcNow,
                LastModifiedAtUtc = null
            };

            dbContext.ArticleComments.Add(comment);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Données de démonstration initialisées.");
        }
    }
}
