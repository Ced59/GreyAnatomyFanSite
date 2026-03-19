using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Application.Members.Commands.LoginMember;
using GreyAnatomyFanSite.Application.Members.Commands.RegisterMember;
using GreyAnatomyFanSite.Web.ViewModels.Membres;
using GreyAnatomyFanSite.Web.ViewModels.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers
{
    public sealed class MembresController : AppControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly ISender sender;

        public MembresController(
            IIdentityService identityService,
            ISender sender)
            : base(identityService)
        {
            this.identityService = identityService;
            this.sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Register(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            RegisterPageViewModel viewModel = new RegisterPageViewModel();
            return View("Register", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(
            string pseudo,
            string mail,
            string password,
            string cPassword,
            CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            IdentityOperationResult result = await sender.Send(
                new RegisterMemberCommand(pseudo, mail, password, cPassword),
                cancellationToken);

            if (!result.Succeeded)
            {
                RegisterPageViewModel errorViewModel = new RegisterPageViewModel
                {
                    Pseudo = pseudo,
                    Mail = mail,
                    Errors = result.Errors
                };

                return View("Register", errorViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? typePubli = null, int? idPubli = null, CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            LoginPageViewModel viewModel = new LoginPageViewModel
            {
                TypePubli = typePubli,
                IdPubli = idPubli
            };

            return View("Login", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(
            string mail,
            string password,
            string? typePubli = null,
            int? idPubli = null,
            CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            IdentityOperationResult result = await sender.Send(
                new LoginMemberCommand(mail, password),
                cancellationToken);

            if (!result.Succeeded)
            {
                LoginPageViewModel errorViewModel = new LoginPageViewModel
                {
                    Mail = mail,
                    Errors = result.Errors,
                    TypePubli = typePubli,
                    IdPubli = idPubli
                };

                return View("Login", errorViewModel);
            }

            if (string.Equals(typePubli, "article", StringComparison.OrdinalIgnoreCase) && idPubli.HasValue)
            {
                return RedirectToAction("ViewArticle", "Home", new { id = idPubli.Value });
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> LogOut(CancellationToken cancellationToken = default)
        {
            await identityService.SignOutAsync();
            await PopulateLayoutAsync(cancellationToken);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Show(string pseudo, CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = $"Profil de {pseudo}",
                Message = "La page de profil membre sera migrée dans un lot dédié à la gestion du compte et des avatars."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }

        public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken = default)
        {
            await PopulateLayoutAsync(cancellationToken);

            FeatureNotReadyViewModel viewModel = new FeatureNotReadyViewModel
            {
                Title = "Mot de passe oublié",
                Message = "Le parcours de réinitialisation du mot de passe sera raccordé à Identity dans un prochain lot."
            };

            return View("~/Views/Shared/FeatureNotYetMigrated.cshtml", viewModel);
        }
    }
}
