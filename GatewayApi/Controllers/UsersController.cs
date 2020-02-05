using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.Core.ExternalConsumer;
using Gateway.Shared.Commands;
using Gateway.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GatewayApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IExternalCommandProducer _producer;

        public UsersController(IConfiguration configuration, IExternalCommandProducer producer)
        {
            _configuration = configuration;
            _producer = producer;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] ValidUser command)
        {
            await _producer.Publish(command);
            return Ok();
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
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private List<UserVm> Users = new List<UserVm>
        {
            new UserVm
            {
                Name = "jmrowca", Description = "Jakub Mrowca", CompanyName = "Abis", Email = "jmrowca@abis.krakow.pl",
                Id = 1, Role = "Worker"
            },
            new UserVm
            {
                Name = "jengel", Description = "Jerzy Engel", CompanyName = "Engel solution", Email = "", Id = 2,
                Role = "User"
            },
            new UserVm
            {
                Name = "admin", Description = "Administrator", CompanyName = "", Email = "", Id = 3,
                Role = "Administrator"
            },
        };

        public UserVm GetUser(string name)
        {
            return Users.Select(x => new UserVm
            {
                CompanyName = x.CompanyName,
                Name = x.Name
                ,
                Description = x.Description,
                Role = x.Role,
                Email = x.Email,
                Id = x.Id
            }).FirstOrDefault(x => x.Name == name);
        }

        public List<UserVm> GetAll()
        {
            return Users;
        }
    }
}
