using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.SignalR.Hubs;
using Gateway.Shared.Events;
using Gateway.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GatewayApi.Services.EventHandlers
{
    public class UserValidEventHandler : INotificationHandler<UserValid>
    {
        private readonly IHubContext<UserHub> _hub;
        private readonly IConfiguration _configuration;

        public UserValidEventHandler(IHubContext<UserHub> hub, IConfiguration configuration)
        {
            _hub = hub;
            _configuration = configuration;
        }

        public async Task Handle(UserValid notification, CancellationToken cancellationToken)
        {
            var token = CreateToken(notification.User);
            notification.User.Token = token;

            var obj = new object[2];
            obj[0] = notification.GetType().Name;
            obj[1] = notification.User;

            await _hub.Clients.Client(notification.ConnectionId).SendCoreAsync("EventEmited", obj);
        }

        public string CreateToken(UserVm user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = _configuration.GetSection("JWT");
            var secretKey = appSettingsSection.GetValue<string>("Secret");
            IdentityModelEventSource.ShowPII = true;
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Rola", user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
