using hamada.Repo;
using hamada.RequestModels;
using hamada.ResponseModels;
using hamada.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserRepository repo,IPasswordService passwordService, IConfiguration config) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseModel>> Register([FromBody] RegisterRequestModel request)
        { 
            var existingUser = await repo.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var hashedPassword = passwordService.HashPassword(request.Password);
            var newUser = await repo.CreateUser(request.Username, hashedPassword, request.Email, request.Phone);

            var response = new RegisterResponseModel
            {
                Id = newUser.Id.ToString(),
                Username = newUser.Username
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel request)
        {
            var existingUser = await repo.GetUserByUsername(request.username);
            if (existingUser == null || !passwordService.VerifyPassword(existingUser.Password, request.password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(existingUser.Username!, existingUser.Role ?? "User");
            return Ok(new LoginResponseModel
            {
                Id = existingUser.Id.ToString(),
                Username = existingUser.Username,
                Token = token
            });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = config.GetSection("jwt");
            var keyString = jwtSettings["key"]!;

            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(keyString);
            }
            catch (FormatException)
            {
                keyBytes = Encoding.UTF8.GetBytes(keyString);
            }

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["issuer"],
                audience: jwtSettings["audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
