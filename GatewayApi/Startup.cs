using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Core;
using App.Core.Commands;
using App.Core.Events;
using App.Core.ExternalConsumer;
using App.Core.ExternalConsumer.Kafka;
using App.Core.ExternalConsumer.RabbitMq;
using App.Core.Queries;
using App.SignalR.Hubs;
using Gateway.Shared.Events;
using GatewayApi.Services.EventHandlers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Ocelot.Authorisation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace GatewayApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCors(services);
            services.AddSignalR(options => options.EnableDetailedErrors = true);
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureMediatR(services);
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<IEventBus, EventBus>();
            services.AddScoped<IWolneRzarty, WolneRzarty>();
            services.AddScoped<INotificationHandler<UserValid>, UserValidEventHandler>();
            services.AddScoped<IExternalEventProducer, KafkaProducer>();
            services.AddScoped<IExternalCommandProducer, RabbitMqProducer>();
            services.AddSingleton<IExternalEventConsumer, KafkaConsumer>();
            services.AddHostedService<ExternalEventConsumerBackgroundWorker>();
            var appSettingsSection = Configuration.GetSection("JWT");
            var secretKey = appSettingsSection.GetValue<string>("Secret");

            var key = Encoding.ASCII.GetBytes(secretKey);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("ApiSecurity", x =>
                {
                    //x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddOcelot(Configuration);

        }

        private void ConfigureMediatR(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ServiceFactory>(p => p.GetService);
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
                builder.AllowAnyHeader()
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowCredentials()));
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var configuration = new OcelotPipelineConfiguration
            {
                AuthorisationMiddleware = async (ctx, next) =>
                {
                    if (this.Authorize(ctx))
                        await next.Invoke();
                    else
                        ctx.Errors.Add(new UnauthorisedError($"Fail to authorize"));
                }
            };
            app.UseCors("MyPolicy");

            app.UseSignalR(routes => { routes.MapHub<UserHub>("/userHub"); });

            app.UseAuthentication();

            app.UseMvc();
            await app.UseOcelot(configuration);
        }

        private bool Authorize(DownstreamContext ctx)
        {
            if (ctx.DownstreamReRoute.AuthenticationOptions.AuthenticationProviderKey == null) return true;
            else
            {
                //flag for authorization
                bool auth = false;

                //where are stored the claims of the jwt token
                Claim[] claims = ctx.HttpContext.User.Claims.ToArray<Claim>();

                //where are stored the required claims for the route
                Dictionary<string, string> required = ctx.DownstreamReRoute.RouteClaimsRequirement;
                Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
                MatchCollection matches;

                Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
                MatchCollection matchesand;
                int cont = 0;
                foreach (KeyValuePair<string, string> claim in required)
                {
                    matches = reor.Matches(claim.Value);
                    foreach (Match match in matches)
                    {
                        matchesand = reand.Matches(match.Value);
                        cont = 0;
                        foreach (Match m in matchesand)
                        {
                            foreach (Claim cl in claims)
                            {
                                if (cl.Type == claim.Key)
                                {
                                    if (cl.Value == m.Value)
                                    {
                                        cont++;
                                    }
                                }
                            }
                        }

                        if (cont == matchesand.Count)
                        {
                            auth = true;
                            break;
                        }
                    }
                }
                return auth;
            }
        }
    }
}
