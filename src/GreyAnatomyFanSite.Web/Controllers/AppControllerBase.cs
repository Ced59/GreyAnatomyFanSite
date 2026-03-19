using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public abstract class AppControllerBase : Controller
{
    private readonly IIdentityService identityService;

    protected AppControllerBase(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    protected async Task PopulateLayoutAsync(CancellationToken cancellationToken = default)
    {
        ViewBag.NbreVisitUnique = 0;
        ViewBag.NbrePagesVues = 0;

        if (Request.Cookies["ConsentCookies"] is null)
        {
            Response.Cookies.Append(
                "ConsentCookies",
                "ok",
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(365),
                    HttpOnly = true,
                    IsEssential = true
                });

            ViewBag.ConsentCookies = "non";
        }
        else
        {
            ViewBag.ConsentCookies = "ok";
        }

        GreyAnatomyFanSite.Application.Common.Models.CurrentMemberSummary? currentMember =
            await identityService.GetCurrentMemberAsync(cancellationToken);

        if (currentMember is null)
        {
            ViewBag.Logged = false;
            return;
        }

        ViewBag.Logged = true;
        ViewBag.Pseudo = currentMember.Pseudo;
        ViewBag.Avatar = currentMember.AvatarPath ?? "images/Avatars/Default.jpg";
        ViewBag.Statut = currentMember.Roles.Contains(ApplicationRoles.Administrator)
            ? ApplicationRoles.Administrator
            : currentMember.Roles.Contains(ApplicationRoles.Heart)
                ? ApplicationRoles.Heart
                : ApplicationRoles.Member;
        ViewBag.MessageBonjour = currentMember.Roles.Contains(ApplicationRoles.Heart)
            ? "mon Coeur"
            : currentMember.Pseudo;
    }
}
