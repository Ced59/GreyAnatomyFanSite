namespace GreyAnatomyFanSite.Application.Common.Models
{
    public sealed class RegisterMemberRequest
    {
        public string Pseudo { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;
    }
}
