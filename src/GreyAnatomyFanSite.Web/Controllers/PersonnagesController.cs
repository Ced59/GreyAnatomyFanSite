using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class PersonnagesController : AppControllerBase
{
    public PersonnagesController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Personnages",
            Message = "Le module des personnages sera migré dans le lot série et personnages."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }


}
