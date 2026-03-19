namespace GreyAnatomyFanSite.Application.Common.Models;

public sealed class IdentityOperationResult
{
    public static IdentityOperationResult Success()
    {
        return new IdentityOperationResult
        {
            Succeeded = true
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

    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
