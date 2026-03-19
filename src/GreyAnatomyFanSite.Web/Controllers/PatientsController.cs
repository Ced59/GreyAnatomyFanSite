using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class PatientsController : AppControllerBase
    {
        public PatientsController(IIdentityService identityService)
            : base(identityService)
        {
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = "Patients",
                Message = "Le module des patients sera migré plus tard, après la migration des entités personnages/relations."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }


    }
}
