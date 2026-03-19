using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using GreyAnatomyFanSite.Application.Members.Commands.ResetPassword;
using Xunit;

namespace GreyAnatomyFanSite.Tests.Unit;

public sealed class ResetPasswordCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordsDoNotMatch()
    {
        FakeIdentityService identityService = new FakeIdentityService();
        ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(identityService);

        IdentityOperationResult result = await handler.Handle(
            new ResetPasswordCommand(Guid.NewGuid(), "token", "Password123!", "Mismatch123!"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Contains("Merci de saisir le même mot de passe!", result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldCallIdentityService_WhenRequestIsValid()
    {
        FakeIdentityService identityService = new FakeIdentityService();
        ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(identityService);

        IdentityOperationResult result = await handler.Handle(
            new ResetPasswordCommand(Guid.NewGuid(), "token", "Password123!", "Password123!"),
            CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.True(identityService.ResetWasCalled);
    }

    private sealed class FakeIdentityService : IIdentityService
    {
        public bool ResetWasCalled { get; private set; }

        public Task<IdentityOperationResult> ConfirmEmailAsync(Guid userId, string encodedToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityOperationResult.Success());
        }

        public Task<CurrentMemberSummary?> GetCurrentMemberAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<CurrentMemberSummary?>(null);
        }

        public Task<MemberProfileDetails?> GetMemberProfileByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.FromResult<MemberProfileDetails?>(null);
        }

        public Task<MemberProfileDetails?> GetMemberProfileByPseudoAsync(string pseudo, CancellationToken cancellationToken)
        {
            return Task.FromResult<MemberProfileDetails?>(null);
        }

        public Task<IdentityOperationResult> PasswordSignInAsync(string email, string password, bool isPersistent, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityOperationResult.Success());
        }

        public Task<IdentityOperationResult> RegisterAsync(RegisterMemberRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityOperationResult.Success());
        }

        public Task<IdentityOperationResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityOperationResult.Success());
        }

        public Task<IdentityOperationResult> ResetPasswordAsync(Guid userId, string encodedToken, string newPassword, CancellationToken cancellationToken)
        {
            ResetWasCalled = true;
            return Task.FromResult(IdentityOperationResult.Success());
        }

        public Task SignOutAsync()
        {
            return Task.CompletedTask;
        }

        public Task<IdentityOperationResult> UpdateCurrentMemberAvatarAsync(string avatarPath, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityOperationResult.Success());
        }
    }
}
