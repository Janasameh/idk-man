using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.Data;
using hamada.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace hamada.Repo
{
    public class UserRepository(AppDbContext Context) : IUserRepository
    {
        public async Task<User> CreateUser(string username, string password, string Email, string Phone)
        {
            User newUser = new()
            {
                Username = username,
                Password = password,
                Email = Email,
                Phone = Phone,

            };
            await Context.AddAsync(newUser);
            await Context.SaveChangesAsync();
            return newUser;

        }

        public async Task<User?> GetUser(int id)
        {

            return await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

        }
        public async Task<User?> GetUserByUsername(string username)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>?> GetUsers()
        {
            return await Context.Users.ToListAsync();
        }

    }
}