using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.RegisterMember;

public sealed record RegisterMemberCommand(
    string Pseudo,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<IdentityOperationResult>;

public sealed class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public RegisterMemberCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
        {
            return IdentityOperationResult.Failure("Les mots de passe ne correspondent pas.");
        }

        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            return IdentityOperationResult.Failure("Le pseudo est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return IdentityOperationResult.Failure("L'adresse email est obligatoire.");
        }

        RegisterMemberRequest registerRequest = new RegisterMemberRequest
        {
            Pseudo = request.Pseudo.Trim(),
            Email = request.Email.Trim(),
            Password = request.Password
        };

        return await identityService.RegisterAsync(registerRequest, cancellationToken);
    }
}
