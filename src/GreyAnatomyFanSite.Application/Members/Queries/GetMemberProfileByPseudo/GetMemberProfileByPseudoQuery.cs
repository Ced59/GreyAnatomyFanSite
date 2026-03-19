using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Queries.GetMemberProfileByPseudo;

public sealed record GetMemberProfileByPseudoQuery(string Pseudo) : IRequest<MemberProfileDetails?>;

public sealed class GetMemberProfileByPseudoQueryHandler : IRequestHandler<GetMemberProfileByPseudoQuery, MemberProfileDetails?>
{
    private readonly IIdentityService identityService;

    public GetMemberProfileByPseudoQueryHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<MemberProfileDetails?> Handle(GetMemberProfileByPseudoQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            return null;
        }

        return await identityService.GetMemberProfileByPseudoAsync(request.Pseudo.Trim(), cancellationToken);
    }
}
