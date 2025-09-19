using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.models;
using hamada.Repo;
using hamada.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace hamada.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserRepository repo) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<User>?>> GetUsers()
        {
            return await repo.GetUsers();
        }

        [HttpGet]
        [Route("/userprofile/{id}")]
        public async Task<ActionResult<User?>> GetUser([FromRoute] int id)
        {
            return await repo.GetUser(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> CreateUser( [FromBody] CreateUserRequestModel request )
        {
          
            return await repo.CreateUser(request.Username, request.Password);
        }

    }
}