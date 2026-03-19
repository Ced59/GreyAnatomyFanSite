using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class ActeursController : AppControllerBase
{
    public ActeursController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Acteurs",
            Message = "Le module des acteurs sera migré dans un lot dédié aux personnages et à la série."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }


}
