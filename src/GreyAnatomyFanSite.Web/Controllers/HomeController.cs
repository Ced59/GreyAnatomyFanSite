using GreyAnatomyFanSite.Application.Articles.Queries.GetArticleDetails;
using GreyAnatomyFanSite.Application.Articles.Queries.GetHomePage;
using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.Infrastructure;
using GreyAnatomyFanSite.Web.ViewModels.Comments;
using GreyAnatomyFanSite.Web.ViewModels.Home;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class HomeController : AppControllerBase
{
    private readonly ISender sender;

    public HomeController(
        IIdentityService identityService,
        ISender sender)
        : base(identityService)
    {
        this.sender = sender;
    }

    public async Task<IActionResult> Index(int pagination = 1, int category = 0, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        HomePageDto dto = await sender.Send(new GetHomePageQuery(pagination, category), cancellationToken);

        HomeIndexPageViewModel viewModel = new HomeIndexPageViewModel
        {
            ActiveCategory = dto.ActiveCategoryId,
            CategoryArticles = dto.Categories
                .Select(categoryDto => new ArticleCategoryOptionViewModel
                {
                    Id = categoryDto.Id,
                    TitreCategory = categoryDto.Title
                })
                .ToList(),
            Articles = dto.Articles
                .Select(articleDto => new HomeArticleSummaryViewModel
                {
                    Id = articleDto.Id,
                    Titre = articleDto.Title,
                    Texte = articleDto.Content,
                    Media = articleDto.MediaPath,
                    TypeMedia = articleDto.MediaType,
                    Date = articleDto.PublishedAtUtc.ToLocalTime(),
                    CommentairesCount = articleDto.CommentCount,
                    Categorie = new ArticleCategoryOptionViewModel
                    {
                        Id = dto.Categories.FirstOrDefault(categoryDto => categoryDto.Title == articleDto.CategoryTitle)?.Id ?? 0,
                        TitreCategory = articleDto.CategoryTitle
                    }
                })
                .ToList(),
            BirthDatesActeurs = dto.Birthdays
                .Select(birthdayDto => new BirthdayActorViewModel
                {
                    ActorFirstName = birthdayDto.ActorFirstName,
                    ActorLastName = birthdayDto.ActorLastName,
                    CharacterFirstName = birthdayDto.CharacterFirstName,
                    CharacterLastName = birthdayDto.CharacterLastName,
                    DateNaissance = birthdayDto.DateOfBirth
                })
                .ToList(),
            PagePagination = dto.CurrentPage,
            NbrePagePagination = dto.TotalPages
        };

        ViewData["Title"] = "Grey's Anatomy";
        ViewData["MetaKeywords"] = "Grey's Anatomy, actualités, série";
        ViewData["MetaDescription"] = "Toute l'actualité Grey's Anatomy conservée avec le design historique et un nouveau socle .NET 10.";

        return View("Index", viewModel);
    }

    public async Task<IActionResult> ViewArticle(int id, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        ArticleDetailsDto? dto = await sender.Send(new GetArticleDetailsQuery(id), cancellationToken);

        if (dto is null)
        {
            return NotFound();
        }

        ArticleDetailsPageViewModel viewModel = new ArticleDetailsPageViewModel
        {
            Article = new ArticleViewModel
            {
                Id = dto.Id,
                Titre = dto.Title,
                Texte = TextFormatting.ToSafeHtml(dto.Content),
                Media = dto.MediaPath,
                TypeMedia = dto.MediaType,
                Date = dto.PublishedAtUtc.ToLocalTime(),
                AuteurPseudo = dto.AuthorPseudo,
                CategoryTitle = dto.CategoryTitle
            },
            Commentaires = dto.Comments
                .Select(commentDto => new ArticleCommentViewModel
                {
                    Titre = commentDto.Title,
                    Text = TextFormatting.ToSafeHtml(commentDto.Content),
                    Date = commentDto.CreatedAtUtc.ToLocalTime(),
                    AuteurPseudo = commentDto.AuthorPseudo,
                    AuteurAvatar = commentDto.AuthorAvatarPath ?? "images/Avatars/Default.jpg"
                })
                .ToList(),
            Articles = dto.LatestArticles
                .Select(articleDto => new RelatedArticleViewModel
                {
                    Id = articleDto.Id,
                    Titre = articleDto.Title,
                    Texte = articleDto.Content,
                    Media = articleDto.MediaPath,
                    TypeMedia = articleDto.MediaType
                })
                .ToList(),
            IsLogged = ViewBag.Logged == true,
            LoginEmail = null,
            Errors = ReadCommentErrors()
        };

        ViewData["Title"] = viewModel.Article.Titre;
        ViewData["MetaKeywords"] = viewModel.Article.Titre;
        ViewData["MetaDescription"] = dto.Content;
        ViewData["MetaFacebookUrl"] = "<meta property=\"og:url\" content=\"https://localhost/Home/ViewArticle/" + viewModel.Article.Id + "\" />";
        ViewData["MetaFacebookType"] = "<meta property=\"og:type\" content=\"article\" />";
        ViewData["MetaFacebookTitle"] = "<meta property=\"og:title\" content=\"" + viewModel.Article.Titre + "\" />";
        ViewData["MetaFacebookDescription"] = "<meta property=\"og:description\" content=\"" + dto.Content.Replace("\n", " ").Trim() + "\" />";
        ViewData["MetaFacebookMedia"] = "<meta property=\"og:image\" content=\"https://localhost/" + (viewModel.Article.Media ?? "images/SiteImg/bannieregreysanatomy.jpg") + "\" />";

        return View("ViewArticle", viewModel);
    }

    public async Task<IActionResult> Articles(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Administration des articles",
            Message = "Le socle Clean Architecture est en place. La page d'administration des articles sera migrée dans le lot suivant."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }

    public IActionResult Error()
    {
        return View();
    }

    private IReadOnlyCollection<string> ReadCommentErrors()
    {
        object? value = TempData["CommentErrors"];

        if (value is null)
        {
            return Array.Empty<string>();
        }

        string raw = Convert.ToString(value) ?? string.Empty;

        return raw
            .Split("||", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
    }
}

