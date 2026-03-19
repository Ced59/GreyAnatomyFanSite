using System.Security.Claims;
using GreyAnatomyFanSite.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GreyAnatomyFanSite.Infrastructure.Services
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                ClaimsPrincipal? principal = httpContextAccessor.HttpContext?.User;
                string? value = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(value, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                ClaimsPrincipal? principal = httpContextAccessor.HttpContext?.User;
                return principal?.Identity?.IsAuthenticated == true;
            }
        }

        public IReadOnlyCollection<string> Roles
        {
            get
            {
                ClaimsPrincipal? principal = httpContextAccessor.HttpContext?.User;

                if (principal is null)
                {
                    return Array.Empty<string>();
                }

                List<string> roles = principal.Claims
                    .Where(claim => claim.Type == ClaimTypes.Role)
                    .Select(claim => claim.Value)
                    .ToList();

                return roles;
            }
        }
    }
}
