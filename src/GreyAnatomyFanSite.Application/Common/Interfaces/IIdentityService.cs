using GreyAnatomyFanSite.Application.Common.Models;

namespace GreyAnatomyFanSite.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<IdentityOperationResult> RegisterAsync(RegisterMemberRequest request, CancellationToken cancellationToken);

    Task<IdentityOperationResult> PasswordSignInAsync(string email, string password, bool isPersistent, CancellationToken cancellationToken);

    Task<IdentityOperationResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken);

    Task<IdentityOperationResult> ResetPasswordAsync(Guid userId, string encodedToken, string newPassword, CancellationToken cancellationToken);

    Task<IdentityOperationResult> ConfirmEmailAsync(Guid userId, string encodedToken, CancellationToken cancellationToken);

    Task<IdentityOperationResult> UpdateCurrentMemberAvatarAsync(string avatarPath, CancellationToken cancellationToken);

    Task<MemberProfileDetails?> GetMemberProfileByPseudoAsync(string pseudo, CancellationToken cancellationToken);

    Task<MemberProfileDetails?> GetMemberProfileByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task SignOutAsync();

    Task<CurrentMemberSummary?> GetCurrentMemberAsync(CancellationToken cancellationToken);
}
