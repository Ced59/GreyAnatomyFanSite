using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Application.Members.Commands.ConfirmMemberEmail;
using GreyAnatomyFanSite.Application.Members.Commands.LoginMember;
using GreyAnatomyFanSite.Application.Members.Commands.RegisterMember;
using GreyAnatomyFanSite.Application.Members.Commands.RequestPasswordReset;
using GreyAnatomyFanSite.Application.Members.Commands.ResetPassword;
using GreyAnatomyFanSite.Application.Members.Commands.UpdateCurrentMemberAvatar;
using GreyAnatomyFanSite.Application.Members.Queries.GetMemberProfileByPseudo;
using GreyAnatomyFanSite.Domain.Constants;
using GreyAnatomyFanSite.Web.ViewModels.Membres;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Web.Controllers;

public sealed class MembresController : AppControllerBase
{
    private const long AvatarMaxLengthInBytes = 1_000_000;

    private readonly IIdentityService identityService;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly ISender sender;

    public MembresController(
        IIdentityService identityService,
        IWebHostEnvironment webHostEnvironment,
        ISender sender)
        : base(identityService)
    {
        this.identityService = identityService;
        this.webHostEnvironment = webHostEnvironment;
        this.sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Register(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        return View("Register", new RegisterPageViewModel());
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
            return View(
                "Register",
                new RegisterPageViewModel
                {
                    Pseudo = pseudo,
                    Mail = mail,
                    Errors = result.Errors
                });
        }

        return View(
            "SendMailRegistration",
            new RegistrationMailSentViewModel
            {
                Mail = mail,
                SuccessMessage = result.SuccessMessage,
                DebugActionUrl = result.DebugActionUrl,
                DebugActionLabel = result.DebugActionLabel
            });
    }

    [HttpGet]
    public async Task<IActionResult> Confirm(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        IdentityOperationResult result = await sender.Send(
            new ConfirmMemberEmailCommand(userId, token),
            cancellationToken);

        return View(
            "Confirm",
            new ConfirmMemberViewModel
            {
                Succeeded = result.Succeeded,
                Message = result.Succeeded
                    ? result.SuccessMessage ?? "Votre compte est désormais activé."
                    : result.Errors.FirstOrDefault() ?? "Echec de l'identification. Essayez de copier le lien directement dans votre navigateur."
            });
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? typePubli = null, int? idPubli = null, string? successMessage = null, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        return View(
            "Login",
            new LoginPageViewModel
            {
                TypePubli = typePubli,
                IdPubli = idPubli,
                SuccessMessage = successMessage
            });
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
            return View(
                "Login",
                new LoginPageViewModel
                {
                    Mail = mail,
                    Errors = result.Errors,
                    TypePubli = typePubli,
                    IdPubli = idPubli
                });
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
    public async Task<IActionResult> Show(string pseudo, string? successMessage = null, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        MemberProfileDetails? member = await sender.Send(new GetMemberProfileByPseudoQuery(pseudo), cancellationToken);

        if (member is null)
        {
            return NotFound();
        }

        bool isCurrentMember = string.Equals(Convert.ToString(ViewBag.Pseudo), member.Pseudo, StringComparison.OrdinalIgnoreCase);
        bool canSeePrivateInformation = isCurrentMember
            || string.Equals(Convert.ToString(ViewBag.Statut), ApplicationRoles.Administrator, StringComparison.Ordinal)
            || string.Equals(Convert.ToString(ViewBag.Statut), ApplicationRoles.Heart, StringComparison.Ordinal);

        return View(
            "ShowMembre",
            new ShowMemberViewModel
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                AvatarPath = member.AvatarPath ?? "images/Avatars/Default.jpg",
                Status = member.Status,
                RegisteredAtUtc = member.RegisteredAtUtc,
                Email = canSeePrivateInformation ? member.Email : null,
                CanSeePrivateInformation = canSeePrivateInformation,
                IsCurrentMember = isCurrentMember,
                SuccessMessage = successMessage
            });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ModifAvatar(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        CurrentMemberSummary? currentMember = await identityService.GetCurrentMemberAsync(cancellationToken);

        if (currentMember is null)
        {
            return RedirectToAction("Login");
        }

        return View(
            "ModifAvatar",
            new ModifyAvatarPageViewModel
            {
                AvatarPath = currentMember.AvatarPath ?? "images/Avatars/Default.jpg"
            });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ModifAvatarPost(IFormFile? image, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        CurrentMemberSummary? currentMember = await identityService.GetCurrentMemberAsync(cancellationToken);

        if (currentMember is null)
        {
            return RedirectToAction("Login");
        }

        List<string> errors = ValidateAvatar(image);

        if (errors.Count > 0)
        {
            return View(
                "ModifAvatar",
                new ModifyAvatarPageViewModel
                {
                    AvatarPath = currentMember.AvatarPath ?? "images/Avatars/Default.jpg",
                    Errors = errors
                });
        }

        string avatarPath = await SaveAvatarAsync(currentMember.Id, image!, cancellationToken);
        IdentityOperationResult result = await sender.Send(new UpdateCurrentMemberAvatarCommand(avatarPath), cancellationToken);

        if (!result.Succeeded)
        {
            return View(
                "ModifAvatar",
                new ModifyAvatarPageViewModel
                {
                    AvatarPath = currentMember.AvatarPath ?? "images/Avatars/Default.jpg",
                    Errors = result.Errors
                });
        }

        return RedirectToAction("Show", new { pseudo = currentMember.Pseudo, successMessage = result.SuccessMessage });
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        return View("ForgotPassword", new ForgotPasswordPageViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePasswordPost(string mail, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        IdentityOperationResult result = await sender.Send(new RequestPasswordResetCommand(mail), cancellationToken);

        return View(
            "ForgotPassword",
            new ForgotPasswordPageViewModel
            {
                Mail = mail,
                Errors = result.Succeeded ? Array.Empty<string>() : result.Errors,
                SuccessMessage = result.SuccessMessage,
                DebugActionUrl = result.DebugActionUrl,
                DebugActionLabel = result.DebugActionLabel
            });
    }

    [HttpGet]
    public async Task<IActionResult> ReinitializeLostPassWord(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        MemberProfileDetails? targetMember = await identityService.GetMemberProfileByIdAsync(userId, cancellationToken);

        return View(
            "InitializeNewPassWord",
            new ResetPasswordPageViewModel
            {
                UserId = userId,
                Token = token,
                Pseudo = targetMember?.Pseudo ?? "Membre"
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InitializeNewPassWordPost(
        Guid userId,
        string token,
        string password,
        string cPassword,
        CancellationToken cancellationToken = default)
    {
        await PopulateLayoutAsync(cancellationToken);

        IdentityOperationResult result = await sender.Send(
            new ResetPasswordCommand(userId, token, password, cPassword),
            cancellationToken);

        if (!result.Succeeded)
        {
            MemberProfileDetails? targetMember = await identityService.GetMemberProfileByIdAsync(userId, cancellationToken);

            return View(
                "InitializeNewPassWord",
                new ResetPasswordPageViewModel
                {
                    UserId = userId,
                    Token = token,
                    Pseudo = targetMember?.Pseudo ?? "Membre",
                    Errors = result.Errors
                });
        }

        return RedirectToAction(
            "Login",
            new
            {
                successMessage = result.SuccessMessage
            });
    }

    private static List<string> ValidateAvatar(IFormFile? image)
    {
        List<string> errors = new List<string>();

        if (image is null || image.Length == 0)
        {
            errors.Add("Merci de sélectionner une image.");
            return errors;
        }

        string extension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
        {
            errors.Add("Seuls les fichiers .jpg ou .png sont accceptés");
        }

        if (image.Length > AvatarMaxLengthInBytes)
        {
            errors.Add("Le fichier doit avoir une taille maximale de 1Mo.");
        }

        return errors;
    }

    private async Task<string> SaveAvatarAsync(Guid memberId, IFormFile image, CancellationToken cancellationToken)
    {
        string originalExtension = Path.GetExtension(image.FileName);
        string extension = string.Equals(originalExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)
            ? ".jpg"
            : originalExtension.ToLowerInvariant();
        string uniqueNumber = Guid.NewGuid().ToString("N");
        string relativePath = $"images/Avatars/{memberId}-{uniqueNumber}{extension}";
        string avatarFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "Avatars");
        Directory.CreateDirectory(avatarFolder);
        string physicalPath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

        await using FileStream stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await image.CopyToAsync(stream, cancellationToken);

        return relativePath;
    }
}
