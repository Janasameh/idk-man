using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using hamada.Repo;
using hamada.RequestModels;
using hamada.ResponseModels;
using hamada.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hamada.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserRepository repo, IPasswordService passwordService, IConfiguration config) : ControllerBase
    {
        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestModel request, RegisterResponseModel response)
        {
            var existingUser = await repo.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var hashedPassword = passwordService.HashPassword(request.Password);
            var newUser = await repo.CreateUser(request.Username, hashedPassword, request.Email, request.Phone);
            return Ok(new { response.Id, response.Username });
        }

        public async Task<ActionResult> Login([FromBody] LoginRequestModel request)
        {
            var existingUser = await repo.GetUserByUsername(request.username);
            if (existingUser == null || passwordService.VerifyPassword(existingUser.Password, request.password) == false)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok("welcome dear user");

        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = config.GetSection("jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}