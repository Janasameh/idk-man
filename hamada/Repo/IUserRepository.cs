using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.models;

namespace hamada.Repo
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(string username, string password);
        public Task<User?> GetUser(int id);
        public Task<List<User>?> GetUsers();

    }
}