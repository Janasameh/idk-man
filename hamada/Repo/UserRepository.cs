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
        public async Task<User> CreateUser(string username, string password)
        {
            User newUser = new()
            {
                Username = username,
                Password = password,

            };
            await Context.AddAsync(newUser);
            await Context.SaveChangesAsync();
            return newUser;

        }

        public async Task<User?> GetUser(int id)
        {

            return await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

        }
    }
}