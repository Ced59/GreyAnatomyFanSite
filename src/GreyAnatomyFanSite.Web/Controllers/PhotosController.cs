using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class PhotosController : AppControllerBase
{
    public PhotosController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Photos",
            Message = "Le module photo sera migré dans un lot dédié aux médias."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }


}
