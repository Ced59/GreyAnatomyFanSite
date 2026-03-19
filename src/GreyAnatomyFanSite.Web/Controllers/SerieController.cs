using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class SerieController : AppControllerBase
    {
        public SerieController(IIdentityService identityService)
            : base(identityService)
        {
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = "Séries",
                Message = "Le module série sera migré dans un lot dédié aux séries, saisons et épisodes."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }

        public async Task<IActionResult> ViewSeason(int idSerie, int saison, CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);
            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
            {
                Title = $"Saison {saison}",
                Message = "La page détail d'une saison sera migrée dans un lot dédié."
            });
        }

        public async Task<IActionResult> ViewEpisode(int idSerie, int saison, int episode, CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);
            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", new FeatureNotReadyViewModel
            {
                Title = $"Épisode {episode}",
                Message = "La page détail d'un épisode sera migrée dans un lot dédié."
            });
        }
    }
}
