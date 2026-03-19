using GreyAnatomyFanSite.Application.Common.Models;

namespace GreyAnatomyFanSite.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityOperationResult> RegisterAsync(RegisterMemberRequest request, CancellationToken cancellationToken);

        Task<IdentityOperationResult> PasswordSignInAsync(string email, string password, bool isPersistent, CancellationToken cancellationToken);

        Task SignOutAsync();

        Task<CurrentMemberSummary?> GetCurrentMemberAsync(CancellationToken cancellationToken);
    }
}
