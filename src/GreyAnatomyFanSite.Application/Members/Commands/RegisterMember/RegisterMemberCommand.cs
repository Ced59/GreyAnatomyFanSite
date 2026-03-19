using System.Text.RegularExpressions;
using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Application.Common.Models;
using MediatR;

namespace GreyAnatomyFanSite.Application.Members.Commands.RegisterMember;

public sealed record RegisterMemberCommand(
    string Pseudo,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<IdentityOperationResult>;

public sealed partial class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand, IdentityOperationResult>
{
    private readonly IIdentityService identityService;

    public RegisterMemberCommandHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IdentityOperationResult> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Pseudo))
        {
            errors.Add("Merci de saisir un pseudo!");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors.Add("Merci de saisir une adresse mail!");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add("Merci de saisir un mot de passe!");
        }

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
        {
            errors.Add("Les mots de passe ne correspondent pas.");
        }

        if (errors.Count > 0)
        {
            return IdentityOperationResult.Failure(errors.ToArray());
        }

        if (!EmailRegex().IsMatch(request.Email.Trim()))
        {
            errors.Add("Cette adresse mail n'est pas valide!");
        }

        if (!PseudoRegex().IsMatch(request.Pseudo.Trim()))
        {
            errors.Add("Le pseudo doit contenir entre 4 et 10 caractères alphanumériques.");
        }

        if (!PasswordRegex().IsMatch(request.Password))
        {
            errors.Add("Mot de passe invalide! Il doit être compris entre 8 et 15 caractères et contenir au minimum un chiffre, une majuscule et un caractère spécial");
        }

        if (errors.Count > 0)
        {
            return IdentityOperationResult.Failure(errors.ToArray());
        }

        RegisterMemberRequest registerRequest = new RegisterMemberRequest
        {
            Pseudo = request.Pseudo.Trim(),
            Email = request.Email.Trim(),
            Password = request.Password
        };

        return await identityService.RegisterAsync(registerRequest, cancellationToken);
    }

    [GeneratedRegex(@"^[A-Za-z0-9._-]+@[A-Za-z0-9_-]+\.[A-Za-z]{2,10}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^[A-Za-z0-9]{4,10}$", RegexOptions.CultureInvariant)]
    private static partial Regex PseudoRegex();

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).{8,15}$", RegexOptions.CultureInvariant)]
    private static partial Regex PasswordRegex();
}
