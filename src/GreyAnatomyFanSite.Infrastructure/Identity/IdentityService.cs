using System.Text;
using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Domain.Constants;
using GreyAnatomyFanSite.Domain.Entities;
using GreyAnatomyFanSite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Infrastructure.Identity;

public sealed class IdentityService : IIdentityService
{
    private readonly IAccountMessageService accountMessageService;
    private readonly ICurrentUser currentUser;
    private readonly ApplicationDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly LinkGenerator linkGenerator;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;

    public IdentityService(
        IAccountMessageService accountMessageService,
        ICurrentUser currentUser,
        ApplicationDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        LinkGenerator linkGenerator,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        this.accountMessageService = accountMessageService;
        this.currentUser = currentUser;
        this.dbContext = dbContext;
        this.httpContextAccessor = httpContextAccessor;
        this.linkGenerator = linkGenerator;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    public async Task<CurrentMemberSummary?> GetCurrentMemberAsync(CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
        {
            return null;
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(member => member.Id == currentUser.UserId.Value, cancellationToken);

        if (memberProfile is null)
        {
            return null;
        }

        return new CurrentMemberSummary
        {
            Id = memberProfile.Id,
            AvatarPath = memberProfile.AvatarPath,
            Pseudo = memberProfile.Pseudo,
            Roles = currentUser.Roles
        };
    }

    public async Task<MemberProfileDetails?> GetMemberProfileByPseudoAsync(string pseudo, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Pseudo == pseudo, cancellationToken);

        return await BuildMemberProfileDetailsAsync(user, cancellationToken);
    }

    public async Task<MemberProfileDetails?> GetMemberProfileByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);

        return await BuildMemberProfileDetailsAsync(user, cancellationToken);
    }

    public async Task<IdentityOperationResult> PasswordSignInAsync(
        string email,
        string password,
        bool isPersistent,
        CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return IdentityOperationResult.Failure("Il n'y a pas d'utilisateur avec cette adresse mail / mot de passe.");
        }

        if (!user.IsActive || !user.EmailConfirmed)
        {
            return IdentityOperationResult.Failure("Vous devez confirmer votre adresse Mail pour vous connecter.");
        }

        SignInResult result = await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return IdentityOperationResult.Failure("Il n'y a pas d'utilisateur avec cette adresse mail / mot de passe.");
        }

        return IdentityOperationResult.Success();
    }

    public async Task<IdentityOperationResult> RegisterAsync(RegisterMemberRequest request, CancellationToken cancellationToken)
    {
        ApplicationUser applicationUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            Pseudo = request.Pseudo,
            AvatarPath = "images/Avatars/Default.jpg",
            RegisteredAtUtc = DateTime.UtcNow,
            IsActive = false,
            EmailConfirmed = false
        };

        IdentityResult createResult = await userManager.CreateAsync(applicationUser, request.Password);

        if (!createResult.Succeeded)
        {
            return IdentityOperationResult.Failure(createResult.Errors.Select(error => error.Description).ToArray());
        }

        IdentityResult roleResult = await userManager.AddToRoleAsync(applicationUser, ApplicationRoles.Member);

        if (!roleResult.Succeeded)
        {
            return IdentityOperationResult.Failure(roleResult.Errors.Select(error => error.Description).ToArray());
        }

        MemberProfile memberProfile = new MemberProfile
        {
            Id = applicationUser.Id,
            Email = applicationUser.Email ?? request.Email,
            Pseudo = applicationUser.Pseudo,
            AvatarPath = applicationUser.AvatarPath,
            CreatedAtUtc = applicationUser.RegisteredAtUtc,
            IsActive = false
        };

        await dbContext.MemberProfiles.AddAsync(memberProfile, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        string encodedToken = await GenerateEncodedEmailConfirmationTokenAsync(applicationUser);
        string confirmationUrl = BuildAbsoluteActionUrl(
            actionName: "Confirm",
            values: new Dictionary<string, string?>
            {
                ["userId"] = applicationUser.Id.ToString(),
                ["token"] = encodedToken
            });

        await accountMessageService.SendRegistrationConfirmationAsync(
            request.Email,
            request.Pseudo,
            confirmationUrl,
            cancellationToken);

        return IdentityOperationResult.Success(
            successMessage: $"Un Email de confirmation a été envoyé à {request.Email}",
            debugActionUrl: confirmationUrl,
            debugActionLabel: "Lien de confirmation (développement)");
    }

    public async Task<IdentityOperationResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return IdentityOperationResult.Failure("Il n'y a pas d'utilisateur avec cette adresse mail.");
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(member => member.Id == user.Id, cancellationToken);

        string encodedToken = await GenerateEncodedResetPasswordTokenAsync(user);
        string resetUrl = BuildAbsoluteActionUrl(
            actionName: "ReinitializeLostPassWord",
            values: new Dictionary<string, string?>
            {
                ["userId"] = user.Id.ToString(),
                ["token"] = encodedToken
            });

        await accountMessageService.SendPasswordResetAsync(
            email,
            memberProfile?.Pseudo ?? user.Pseudo,
            resetUrl,
            cancellationToken);

        return IdentityOperationResult.Success(
            successMessage: $"Un mail vous permettant de réinitialiser votre mot de passe a été envoyé à {email}",
            debugActionUrl: resetUrl,
            debugActionLabel: "Lien de réinitialisation (développement)");
    }

    public async Task<IdentityOperationResult> ResetPasswordAsync(Guid userId, string encodedToken, string newPassword, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return IdentityOperationResult.Failure("Lien de réinitialisation invalide.");
        }

        string token = DecodeToken(encodedToken);
        IdentityResult result = await userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            return IdentityOperationResult.Failure(result.Errors.Select(error => error.Description).ToArray());
        }

        return IdentityOperationResult.Success("Changement de mot de passe réussi. Merci de vous identifier.");
    }

    public async Task<IdentityOperationResult> ConfirmEmailAsync(Guid userId, string encodedToken, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return IdentityOperationResult.Failure("Le lien de confirmation est invalide ou expiré.");
        }

        string token = DecodeToken(encodedToken);
        IdentityResult result = await userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            return IdentityOperationResult.Failure("Le lien de confirmation est invalide ou expiré.");
        }

        user.IsActive = true;
        IdentityResult updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return IdentityOperationResult.Failure(updateResult.Errors.Select(error => error.Description).ToArray());
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .SingleOrDefaultAsync(member => member.Id == user.Id, cancellationToken);

        if (memberProfile is not null)
        {
            memberProfile.IsActive = true;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return IdentityOperationResult.Success($"Merci {user.Pseudo} de t'être enregistré sur Grey-Anatomy-Fan.Fr");
    }

    public async Task<IdentityOperationResult> UpdateCurrentMemberAvatarAsync(string avatarPath, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
        {
            return IdentityOperationResult.Failure("Vous devez être connecté pour modifier votre avatar.");
        }

        ApplicationUser? user = await userManager.FindByIdAsync(currentUser.UserId.Value.ToString());

        if (user is null)
        {
            return IdentityOperationResult.Failure("Utilisateur introuvable.");
        }

        user.AvatarPath = avatarPath;
        IdentityResult userUpdateResult = await userManager.UpdateAsync(user);

        if (!userUpdateResult.Succeeded)
        {
            return IdentityOperationResult.Failure(userUpdateResult.Errors.Select(error => error.Description).ToArray());
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .SingleOrDefaultAsync(member => member.Id == currentUser.UserId.Value, cancellationToken);

        if (memberProfile is null)
        {
            return IdentityOperationResult.Failure("Profil membre introuvable.");
        }

        memberProfile.AvatarPath = avatarPath;
        await dbContext.SaveChangesAsync(cancellationToken);

        return IdentityOperationResult.Success("Changement de photo de profil réussi");
    }

    public Task SignOutAsync()
    {
        return signInManager.SignOutAsync();
    }

    private async Task<MemberProfileDetails?> BuildMemberProfileDetailsAsync(ApplicationUser? user, CancellationToken cancellationToken)
    {
        if (user is null)
        {
            return null;
        }

        MemberProfile? memberProfile = await dbContext.MemberProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(member => member.Id == user.Id, cancellationToken);

        if (memberProfile is null)
        {
            return null;
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        return new MemberProfileDetails
        {
            Id = memberProfile.Id,
            Pseudo = memberProfile.Pseudo,
            Email = memberProfile.Email,
            AvatarPath = memberProfile.AvatarPath,
            RegisteredAtUtc = memberProfile.CreatedAtUtc,
            IsActive = user.IsActive && user.EmailConfirmed,
            Status = GetLegacyStatus(user, roles)
        };
    }

    private async Task<string> GenerateEncodedEmailConfirmationTokenAsync(ApplicationUser user)
    {
        string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return EncodeToken(token);
    }

    private async Task<string> GenerateEncodedResetPasswordTokenAsync(ApplicationUser user)
    {
        string token = await userManager.GeneratePasswordResetTokenAsync(user);
        return EncodeToken(token);
    }

    private string BuildAbsoluteActionUrl(string actionName, IDictionary<string, string?> values)
    {
        string? relativePath = linkGenerator.GetPathByAction(
            httpContextAccessor.HttpContext,
            action: actionName,
            controller: "Membres",
            values: values);

        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return string.Empty;
        }

        HttpRequest? request = httpContextAccessor.HttpContext?.Request;

        if (request is null)
        {
            return relativePath;
        }

        return UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, relativePath);
    }

    private static string EncodeToken(string token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(token);
        return WebEncoders.Base64UrlEncode(bytes);
    }

    private static string DecodeToken(string encodedToken)
    {
        byte[] bytes = WebEncoders.Base64UrlDecode(encodedToken);
        return Encoding.UTF8.GetString(bytes);
    }

    private static string GetLegacyStatus(ApplicationUser user, IEnumerable<string> roles)
    {
        if (!user.IsActive || !user.EmailConfirmed)
        {
            return "Inactif";
        }

        if (roles.Contains(ApplicationRoles.Heart))
        {
            return ApplicationRoles.Heart;
        }

        if (roles.Contains(ApplicationRoles.Administrator))
        {
            return ApplicationRoles.Administrator;
        }

        return "Actif";
    }
}
