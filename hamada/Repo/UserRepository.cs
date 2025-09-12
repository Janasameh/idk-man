using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.Data;
using hamada.models;
using Microsoft.EntityFrameworkCore;

namespace hamada.Repo
{
    public class UserRepository(AppDbContext Context) : IUserRepository
    {
        public Task<User> CreateUser(string username, string password)
        {
            var newUser = 
            {
                newUser.Username = username,

            };


        }

        public async Task<User?> GetUser(int id)
        {

            return await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

        }
    }
}