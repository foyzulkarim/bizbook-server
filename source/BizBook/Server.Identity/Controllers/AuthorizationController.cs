using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Server.Identity.Models;

namespace Server.Identity.Controllers
{
    [Authorize]
    [RoutePrefix("api/Authorization")]
    public class AuthorizationController : ApiController
    {
        CacheItemPolicy policy;
        MemoryCache cache;

        public AuthorizationController()
        {
            policy = new CacheItemPolicy
            {
                SlidingExpiration = new TimeSpan(1, 0, 0)
            };

            cache = MemoryCache.Default;
        }

        [HttpPost]
        [Route("Authorize")]
        [ActionName("Authorize")]
        public IHttpActionResult Authorize(PermissionRequest permissionRequest)
        {
            var db = Request.GetOwinContext().Get<SecurityDbContext>();

           // List<ApplicationUser> users = GetApplicationUsers(db);

            //List<ApplicationPermission> permissions;
            //bool containsPermissions = cache.Contains("Permissions");
            //if (containsPermissions)
            //{
            //    permissions = cache.Get("Permissions") as List<ApplicationPermission>;
            //    if (permissions.Count != db.Permissions.Count())
            //    {
            //        permissions = AddApplicationPermissions(db);
            //    }
            //}
            //else
            //{
            //    permissions = AddApplicationPermissions(db);
            //}

            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!user.IsActive)
            {
                return Unauthorized();
            }

            return this.Ok(user);
            //var any = permissions.Any(
            //    x => x.Role.Name == user.RoleName && x.Resource.Name.ToLower() == permissionRequest.Name.ToLower() && x.IsAllowed);
            //return any ? (IHttpActionResult)Ok(user) : Unauthorized();
        }

        private List<ApplicationPermission> AddApplicationPermissions(SecurityDbContext db)
        {
            List<ApplicationPermission> permissions;
            permissions = db.Permissions.Include(x => x.Role).Include(x => x.Resource).ToList().Select(
                x => new ApplicationPermission
                {
                    Id = x.Id,
                    Resource = x.Resource,
                    IsAllowed = x.IsAllowed,
                    RoleId = x.RoleId,
                    ResourceId = x.ResourceId,
                    Role = x.Role,
                    IsDisabled = x.IsDisabled
                }).ToList();
            cache["Permissions"] = permissions;
            return permissions;
        }

        private List<ApplicationUser> GetApplicationUsers(SecurityDbContext db)
        {
            List<ApplicationUser> users;
            users = db.Users.ToList().Select(
                x => new ApplicationUser
                {
                    UserName = x.UserName,
                    RoleName = x.RoleName,
                    Id = x.Id,
                    ShopId = x.ShopId,
                    IsActive = x.IsActive
                }).ToList();
            cache["Users"] = users;
            return users;
        }
    }
}
