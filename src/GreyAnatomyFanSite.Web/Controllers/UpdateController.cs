using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class UpdateController : AppControllerBase
{
    public UpdateController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Updates du site",
            Message = "Les updates du site seront reconnectées une fois le module de contenu administratif migré."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }


}
