using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class AdministrationController : AppControllerBase
{
    public AdministrationController(IIdentityService identityService)
        : base(identityService)
    {
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
        {
            Title = "Administration",
            Message = "Les écrans d'administration détaillés seront migrés progressivement après le socle membres/articles."
        };

        return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
    }

public async Task<IActionResult> Membres(CancellationToken cancellationToken = default)
{
    await PopulateLayoutAsync(cancellationToken);
    return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
    {
        Title = "Administration Membres",
        Message = "Écran non encore migré dans cette première itération."
    });
}

public async Task<IActionResult> Site(CancellationToken cancellationToken = default)
{
    await PopulateLayoutAsync(cancellationToken);
    return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
    {
        Title = "Administration Site",
        Message = "Écran non encore migré dans cette première itération."
    });
}

public async Task<IActionResult> Serie(CancellationToken cancellationToken = default)
{
    await PopulateLayoutAsync(cancellationToken);
    return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
    {
        Title = "Administration Série",
        Message = "Écran non encore migré dans cette première itération."
    });
}
}
