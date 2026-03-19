using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.RequestPasswordReset;

public sealed record RequestPasswordResetCommand(string Email) : IRequest<IdentityOperationResult>;

public sealed class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public RequestPasswordResetCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return IdentityOperationResult.Failure("Merci de saisir une adresse mail.");
        }

        return await identityService.RequestPasswordResetAsync(request.Email.Trim(), cancellationToken);
    }
}
