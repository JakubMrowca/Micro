using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.Events;
using App.Core.ExternalConsumer;
using App.Core.ExternalConsumer.Kafka;
using App.Core.ExternalConsumer.RabbitMq;
using App.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User.Services.CommandHandlers;
using User.Services.EventHandlers;
using User.Shared.Command;
using User.Shared.Events;

namespace UserApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureMediatR(services);
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<IEventBus, EventBus>();
            services.AddScoped<INotificationHandler<UserValid>, UserValidEventHandler>();
            services.AddScoped<IRequestHandler<ValidUser, Unit>, ValidUserCommandHandler>();

            services.AddScoped<IExternalEventProducer, KafkaProducer>();
            services.AddSingleton<IExternalEventConsumer, RabbitMqConsumer>();
            services.AddScoped<IExternalCommandProducer, RabbitMqProducer>();
            //services.AddSingleton<IExternalEventConsumer, KafkaConsumer>();
            services.AddHostedService<ExternalEventConsumerBackgroundWorker>();

            // configure strongly typed settings objects
            //var appSettingsSection = Configuration.GetSection("JWT");
            //var secretKey = appSettingsSection.GetValue<string>("Secret");

            //var key = Encoding.ASCII.GetBytes(secretKey);
            //services.AddAuthentication(x =>
            //    {
            //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrator",
            //        policy => policy.RequireRole("Administrator"));
            //    options.AddPolicy("User",
            //        policy => policy.RequireRole("User"));
            //    options.AddPolicy("Guest",
            //        policy => policy.RequireRole("Guest"));
            //});
        }
        private void ConfigureMediatR(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ServiceFactory>(p => p.GetService);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //app.UseAuthentication();

            app.UseMvc();
        }
    }
}
