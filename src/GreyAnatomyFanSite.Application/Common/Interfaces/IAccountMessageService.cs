namespace GreyAnatomyFanSite.Application.Common.Interfaces;

public interface IAccountMessageService
{
    Task SendRegistrationConfirmationAsync(string email, string pseudo, string confirmationUrl, CancellationToken cancellationToken);

    Task SendPasswordResetAsync(string email, string pseudo, string resetUrl, CancellationToken cancellationToken);
}
