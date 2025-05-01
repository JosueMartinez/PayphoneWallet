using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PayphoneWallet.Entities.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayphoneWallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto credentials)
        {
            if (credentials == null)
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = GenerateJwtToken(credentials);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(LoginDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hIHOejfOKxodIG44G0o6PQBsNypoGIxR"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "PayphoneWallet",
                audience: "users",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
