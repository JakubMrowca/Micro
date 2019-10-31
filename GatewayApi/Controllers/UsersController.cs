using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GatewayApi.Models;
using GatewayApi.Queries;
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

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authorize")]
        public async Task<UserWithToken> Authorize([FromBody] AuthorizeUser query)
        {
            //user authorize method
            var user = new UserWithToken{Name = "JerzyDwa",Description = "JerzyEngel",Role = "User", Id = 10};

            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = _configuration.GetSection("JWT");
            var secretKey = appSettingsSection.GetValue<string>("Secret");
            IdentityModelEventSource.ShowPII = true;
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("seedDbSettings")]
        public async Task<IActionResult> SeedDbSettings()
        {
            var connection = _configuration.GetConnectionString("Bwi");
            //await StartupDataSeeder.SeedDataSettings(_configuration, connection);
            return Ok();
        }
    }
}
