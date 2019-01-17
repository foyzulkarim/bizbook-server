using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Identity.Models;

namespace Server.Identity.Controllers.CommandControllers.System
{
    [RoutePrefix("api/ApplicationUserRoles")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationUserRolesController : ApiController
    {
        private SecurityDbContext db = new SecurityDbContext();

        [HttpPost]
        [Route("Post")]
        [ActionName("Post")]
        // POST: api/ApplicationUserRoles
        public async Task<IHttpActionResult> Post(AppUserRolesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityUserRole userRole = db.ApplicationUserRoles.FirstOrDefault(x => x.UserId == vm.UserId && x.RoleId == vm.RoleId);
            if (userRole == null)
            {
                userRole = db.ApplicationUserRoles.Add(new IdentityUserRole { RoleId = vm.RoleId, UserId = vm.UserId });
                await db.SaveChangesAsync();
            }

            return Ok(userRole.UserId);
        }

        [HttpPut]
        [Route("Put")]
        [ActionName("Put")]
        // PUT: api/ApplicationUserRoles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(AppUserRolesViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityUserRole entity = db.ApplicationUserRoles.FirstOrDefault(x => x.UserId == vm.UserId);
            db.ApplicationUserRoles.Remove(entity);
            db.SaveChanges();
            IdentityUserRole userRole = new IdentityUserRole { RoleId = vm.RoleId, UserId = vm.UserId };
            db.ApplicationUserRoles.Add(userRole);
            await db.SaveChangesAsync();
            return Ok(userRole);
        }
    }
}
