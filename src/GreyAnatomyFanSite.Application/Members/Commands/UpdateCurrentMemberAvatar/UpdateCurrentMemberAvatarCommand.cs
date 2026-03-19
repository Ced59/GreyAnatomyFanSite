using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.UpdateCurrentMemberAvatar;

public sealed record UpdateCurrentMemberAvatarCommand(string AvatarPath) : IRequest<IdentityOperationResult>;

public sealed class UpdateCurrentMemberAvatarCommandHandler : IRequestHandler<UpdateCurrentMemberAvatarCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public UpdateCurrentMemberAvatarCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(UpdateCurrentMemberAvatarCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.AvatarPath))
        {
            return IdentityOperationResult.Failure("Le chemin de l'avatar est obligatoire.");
        }

        return await identityService.UpdateCurrentMemberAvatarAsync(request.AvatarPath, cancellationToken);
    }
}
