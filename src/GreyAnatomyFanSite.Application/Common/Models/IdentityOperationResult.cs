namespace GreyAnatomyFanSite.Application.Common.Models;

public sealed class IdentityOperationResult
{
    public static IdentityOperationResult Success(
        string? successMessage = null,
        string? debugActionUrl = null,
        string? debugActionLabel = null)
    {
        return new IdentityOperationResult
        {
            Succeeded = true,
            SuccessMessage = successMessage,
            DebugActionUrl = debugActionUrl,
            DebugActionLabel = debugActionLabel
        };
    }

    public static IdentityOperationResult Failure(params string[] errors)
    {
        return new IdentityOperationResult
        {
            Succeeded = false,
            Errors = errors
        };
    }

    public bool Succeeded { get; init; }

    public string? SuccessMessage { get; init; }

    public string? DebugActionUrl { get; init; }

    public string? DebugActionLabel { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
