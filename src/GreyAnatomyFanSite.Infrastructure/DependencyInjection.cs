using GreyAnatomyFanSite.Application.Common.Interfaces;
using GreyAnatomyFanSite.Infrastructure.Identity;
using GreyAnatomyFanSite.Infrastructure.Persistence;
using GreyAnatomyFanSite.Infrastructure.Persistence.Seed;
using GreyAnatomyFanSite.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreyAnatomyFanSite.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' est introuvable.");

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services
            .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Membres/Login";
            options.LogoutPath = "/Membres/LogOut";
            options.AccessDeniedPath = "/Membres/Login";
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IAccountMessageService, DevelopmentAccountMessageService>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ApplicationDbInitializer>();

        return services;
    }
}
