using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [RoutePrefix("api/ApplicationUserRoleQuery")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationUserRoleQueryController : ApiController
    {
        private SecurityDbContext db = new SecurityDbContext();

        [Route("Data")]
        [ActionName("Data")]
        [HttpGet]
        public async Task<IHttpActionResult> Data()
        {
            var roles = db.Roles.ToDictionary(x => x.Id, x => x.Name);
            var users = db.Users.ToDictionary(x => x.Id, x => x.Email);
            var identityUserRoles = db.ApplicationUserRoles.ToList().Select(x => new AppUserRolesViewModel { RoleId = x.RoleId, UserId = x.UserId, RoleName = roles[x.RoleId], UserName = users[x.UserId] }).ToList();
            return Ok(identityUserRoles);
        }
    }
}
