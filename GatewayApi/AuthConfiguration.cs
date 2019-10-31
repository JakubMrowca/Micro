using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GatewayApi
{
    public static class AuthConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("JWT");
            var secretKey = appSettingsSection.GetValue<string>("Secret");

            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator",
                    policy => policy.RequireRole("Administrator"));
                options.AddPolicy("User",
                    policy => policy.RequireRole("User"));
                options.AddPolicy("Guest",
                    policy => policy.RequireRole("Guest"));
            });
        }
    }
}
