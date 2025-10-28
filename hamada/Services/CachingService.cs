using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using hamada.models;
using hamada.Repo;

namespace hamada.Services
{
    public class CachingService(IMemoryCache cache, IUserRepository userRepository)
    {
        private const string UsersCacheKey = "users";

        // Note: `cache` and `userRepository` are the primary-constructor parameters and
        // are in scope for methods and member initializers in C# 12+.

        public async Task<List<User>?> GetUsersAsync()
        {
            if (cache.TryGetValue(UsersCacheKey, out List<User>? users) && users is not null)
            {
                Console.WriteLine("Getting from cache!");
                return users;
            }

            Console.WriteLine("Getting from DB...");
            // Call the repository (avoid calling GetUsersAsync on this class to prevent recursion)
            var fetched = await userRepository.GetUsers();

            if (fetched is not null)
            {
                cache.Set(UsersCacheKey, fetched, TimeSpan.FromMinutes(10));
            }

            return fetched;
        }

        public void InvalidateUsersCache()
        {
            cache.Remove(UsersCacheKey);
        }
    }
}