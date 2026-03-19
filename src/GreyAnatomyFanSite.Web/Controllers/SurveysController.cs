using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class SurveysController : AppControllerBase
{
    public SurveysController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Sondages",
            Message = "Le module des sondages sera migré après les articles et les membres."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }

public async Task<IActionResult> Admin(CancellationToken cancellationToken = default)
{
    await PopulateLayoutAsync(cancellationToken);
    return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
    {
        Title = "Administration des sondages",
        Message = "Écran non encore migré dans cette première itération."
    });
}
}
