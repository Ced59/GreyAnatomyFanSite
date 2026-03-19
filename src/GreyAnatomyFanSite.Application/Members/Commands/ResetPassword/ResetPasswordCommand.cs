using System.Text.RegularExpressions;
using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string Token,
    string Password,
    string ConfirmPassword) : IRequest<IdentityOperationResult>;

public sealed partial class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("Merci de saisir un mot de passe!");
        }

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
        {
            errors.Add("Merci de saisir le même mot de passe!");
        }

        if (errors.Count > 0)
        {
            return IdentityOperationResult.Failure(errors.ToArray());
        }

        if (!PasswordRegex().IsMatch(request.Password))
        {
            return IdentityOperationResult.Failure("Mot de passe invalide! Il doit être compris entre 8 et 15 caractères et contenir au minimum un chiffre, une majuscule et un caractère spécial");
        }

        return await identityService.ResetPasswordAsync(request.UserId, request.Token, request.Password, cancellationToken);
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).{8,15}$", RegexOptions.CultureInvariant)]
    private static partial Regex PasswordRegex();
}
