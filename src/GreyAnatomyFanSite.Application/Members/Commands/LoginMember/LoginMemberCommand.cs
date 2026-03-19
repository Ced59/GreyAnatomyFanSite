using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.LoginMember;

public sealed record LoginMemberCommand(string Email, string Password) : IRequest<IdentityOperationResult>;

public sealed class LoginMemberCommandHandler : IRequestHandler<LoginMemberCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public LoginMemberCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(LoginMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return IdentityOperationResult.Failure("Veuillez renseigner votre email et votre mot de passe.");
        }

        return await identityService.PasswordSignInAsync(
            request.Email.Trim(),
            request.Password,
            false,
            cancellationToken);
    }
}
