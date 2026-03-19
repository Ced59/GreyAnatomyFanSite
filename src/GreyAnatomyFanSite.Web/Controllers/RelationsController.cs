using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class RelationsController : AppControllerBase
    {
        public RelationsController(IIdentityService identityService)
            : base(identityService)
        {
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = "Relations",
                Message = "Le module des relations sera migré dans le lot série et personnages."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }


    }
}
