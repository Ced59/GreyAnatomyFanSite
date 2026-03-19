using GreyAnatomyFanSite.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace GreyAnatomyFanSite.Infrastructure.Services;

public sealed class DevelopmentAccountMessageService : IAccountMessageService
{
    private readonly ILogger<DevelopmentAccountMessageService> logger;

    public DevelopmentAccountMessageService(ILogger<DevelopmentAccountMessageService> logger)
    {
        this.logger = logger;
    }

    public Task SendRegistrationConfirmationAsync(string email, string pseudo, string confirmationUrl, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Email de confirmation généré pour {Pseudo} ({Email}). Lien: {ConfirmationUrl}",
            pseudo,
            email,
            confirmationUrl);

        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string email, string pseudo, string resetUrl, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Email de réinitialisation généré pour {Pseudo} ({Email}). Lien: {ResetUrl}",
            pseudo,
            email,
            resetUrl);

        return Task.CompletedTask;
    }
}