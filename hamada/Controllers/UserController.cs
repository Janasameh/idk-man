using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.models;
using hamada.Repo;
using hamada.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using hamada.Services;

namespace hamada.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserRepository repo, hamada.Services.CachingService cachingService) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>?>> GetUsers()
        {
            var users = await cachingService.GetUsersAsync();
            return users;
        }

        [HttpPost("cache/invalidate")]
        public IActionResult InvalidateUsersCache()
        {
            cachingService.InvalidateUsersCache();
            return NoContent();
        }

        [HttpGet]
        [Route("/userprofile/{id}")]
        [Authorize]
        public async Task<ActionResult<User?>> GetUser([FromRoute] int id)
        {
            return await repo.GetUser(id);
        }
    }
}