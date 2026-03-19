using GreyAnatomyFanSite.Domain.Constants;
using GreyAnatomyFanSite.Domain.Entities;
using GreyAnatomyFanSite.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreyAnatomyFanSite.Infrastructure.Persistence.Seed;

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

        await SeedUsersAsync(cancellationToken);
        await SeedContentAsync(cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(
            email: "admin@greys.local",
            pseudo: "AdminGreys",
            password: "Admin123!",
            roles: new[] { ApplicationRoles.Administrator, ApplicationRoles.Member },
            cancellationToken: cancellationToken);

        await EnsureUserExistsAsync(
            email: "coeur@greys.local",
            pseudo: "MonCoeur",
            password: "Coeur123!",
            roles: new[] { ApplicationRoles.Heart, ApplicationRoles.Member },
            cancellationToken: cancellationToken);
    }

    private async Task EnsureUserExistsAsync(
        string email,
        string pseudo,
        string password,
        IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Pseudo = pseudo,
                AvatarPath = "images/Avatars/Default.jpg",
                RegisteredAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            IdentityResult createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"Impossible de créer le compte de démonstration {email}.");
            }

            IdentityResult roleResult = await userManager.AddToRolesAsync(user, roles);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Impossible d'affecter les rôles au compte de démonstration {email}.");
            }
        }
        else
        {
            user.IsActive = true;
            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .SingleOrDefaultAsync(member => member.Id == user.Id, cancellationToken);

        if (memberProfile is null)
        {
            dbContext.MemberProfiles.Add(new MemberProfile
            {
                Id = user.Id,
                Pseudo = user.Pseudo,
                Email = email,
                AvatarPath = user.AvatarPath,
                CreatedAtUtc = user.RegisteredAtUtc,
                IsActive = true
            });

            await dbContext.SaveChangesAsync(cancellationToken);
        }
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
