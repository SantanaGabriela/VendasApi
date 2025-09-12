using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VendasApi.DTOs;

namespace VendasApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] DTOs.LoginDto loginDto)
        {
            // Simulação de autenticação
            if (loginDto.Email == null)
            {

                return Unauthorized("Email is required.");

            }
            if (loginDto.Password == null)
            {
                return Unauthorized("Password is required.");
            }
            if (loginDto.Email != "admin@teste.com" || loginDto.Password != "123")
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("email", loginDto.Email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new TokenResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires ?? DateTime.UtcNow
            });
        }
    }
}

