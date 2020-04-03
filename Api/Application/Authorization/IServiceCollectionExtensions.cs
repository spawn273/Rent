using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RentApi.Infrastructure.Database;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SmartAnalytics.BASF.Backend.Application.Authorization
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection SetUpAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/415
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var authConfigSection = configuration.GetSection("Application:Authorization");
            services.Configure<AuthorizationOptions>(authConfigSection);

            var authOptions = authConfigSection.Get<AuthorizationOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authOptions.Jwt.Issuer,
                        ValidAudience = authOptions.Jwt.Issuer,
                        IssuerSigningKey = authOptions.Jwt.GetSymmetricSecurityKey()
                    };
                });

            return services;
        }
    }
}
