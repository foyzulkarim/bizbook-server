using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [RoutePrefix("api/ApplicationRoleQuery")]
    [Authorize(Roles = "SuperAdmin,ShopAdmin")]
    public class ApplicationRoleQueryController : ApiController
    {
        private SecurityDbContext db;
        [Route("RoleDropdown")]
        [ActionName("RoleDropdown")]
        [HttpGet]
        public async Task<IHttpActionResult> GetApplicationRoleDropdown()
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            var dbRoles = db.ApplicationRoles.AsQueryable();
            if (!User.IsInRole("SuperAdmin"))
            {
                dbRoles = dbRoles.Where(x => x.Name != ApplicationRoles.SuperAdmin.ToString());
            }
            
            var roles = await dbRoles.Select(x => new { x.Id, x.Name }).ToListAsync();
            return Ok(roles);
        }

        [Route("Data")]
        [ActionName("Data")]
        [HttpGet]
        public async Task<IHttpActionResult> Data()
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            var dbRoles = db.ApplicationRoles.AsQueryable();
            if (!User.IsInRole("SuperAdmin"))
            {
                dbRoles = dbRoles.Where(x => x.Name != ApplicationRoles.SuperAdmin.ToString());
            }

            var queryable = await dbRoles.Select(x=>new AppRoleViewModel
            {
                Id = x.Id,Name = x.Name,Description = x.Description,DefaultRoute=x.DefaultRoute
            }).ToListAsync();
            return Ok(queryable);
        }
    }
}
