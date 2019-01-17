using System;
using System.Linq;
using System.Runtime.Caching;
using Server.Identity.Models;

namespace Server.Identity
{
    public class CacheConfig
    {
        public static void Register()
        {
            var cache = MemoryCache.Default;
            var policy = new CacheItemPolicy
                             {
                                 SlidingExpiration = new TimeSpan(1, 0, 0)
                             };
            SecurityDbContext db = SecurityDbContext.Create();
            var roles = db.Roles.ToList().Select(x => new ApplicationRole { Id = x.Id, Name = x.Name }).ToList();
            bool addedRoles = cache.Add("Roles", roles, policy);
            var users = db.Users.ToList().Select(
                x => new ApplicationUser
                {
                             UserName = x.UserName,
                             RoleName = x.RoleName,
                             Id = x.Id,
                             ShopId = x.ShopId,
                             IsActive = x.IsActive
                         }).ToList();
            bool addedUsers = cache.Add("Users", users, policy);
        }
    }
}