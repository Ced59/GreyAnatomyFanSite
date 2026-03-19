using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.ConfirmMemberEmail;

public sealed record ConfirmMemberEmailCommand(Guid UserId, string Token) : IRequest<IdentityOperationResult>;

public sealed class ConfirmMemberEmailCommandHandler : IRequestHandler<ConfirmMemberEmailCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public ConfirmMemberEmailCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(ConfirmMemberEmailCommand request, CancellationToken cancellationToken)
    {
        return await identityService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
    }
}
