using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class VideosController : AppControllerBase
    {
        public VideosController(IIdentityService identityService)
            : base(identityService)
        {
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = "Vidéos",
                Message = "Le module vidéo sera migré dans un lot dédié aux médias."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }


    }
}
