namespace GreyAnatomyFanSite.Domain.Constants;

public static class ApplicationRoles
{
    public const string Administrator = "Administrateur";
    public const string Heart = "Coeur";
    public const string Member = "Membre";

    public static IReadOnlyCollection<string> All { get; } =
        new[]
        {
            Administrator,
            Heart,
            Member
        };
}
