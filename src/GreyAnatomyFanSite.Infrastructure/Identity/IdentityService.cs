using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Domain.Constants;
using GreyAnatomyFanSite.Domain.Entities;
using GreyAnatomyFanSite.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GreyAnatomyFanSite.Infrastructure.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly ICurrentUser currentUser;
        private readonly ApplicationDbContext dbContext;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityService(
            ICurrentUser currentUser,
            ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.currentUser = currentUser;
            this.dbContext = dbContext;
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

        public async Task<IdentityOperationResult> PasswordSignInAsync(
            string email,
            string password,
            bool isPersistent,
            CancellationToken cancellationToken)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(email);

            if (user is null || !user.IsActive)
            {
                return IdentityOperationResult.Failure("Adresse email ou mot de passe invalide.");
            }

            SignInResult result = await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return IdentityOperationResult.Failure("Adresse email ou mot de passe invalide.");
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
                IsActive = true
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
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            await dbContext.MemberProfiles.AddAsync(memberProfile, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await signInManager.SignInAsync(applicationUser, isPersistent: false);

            return IdentityOperationResult.Success();
        }

        public Task SignOutAsync()
        {
            return signInManager.SignOutAsync();
        }
    }
}
