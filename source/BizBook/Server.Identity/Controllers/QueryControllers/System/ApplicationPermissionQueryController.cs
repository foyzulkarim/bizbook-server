using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [RoutePrefix("api/ApplicationPermissionQuery")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationPermissionQueryController : ApiController
    {
        private SecurityDbContext db = new SecurityDbContext();

        [Route("Data")]
        [ActionName("Data")]
        [HttpGet]
        public async Task<IHttpActionResult> Data(string keyword = "")
        {
            IQueryable<ApplicationPermission> queryable = db.Permissions.Include(r => r.Resource).Include(r => r.Role);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                queryable = queryable.Where(
                    x => x.Role.Name.ToLower().Contains(keyword) || x.Resource.Name.ToLower().Contains(keyword) || x.IsAllowed.ToString().ToLower().Contains(keyword));

            }
            var list = await queryable.OrderBy(x => x.Role.Name).ToListAsync();
            List<AppPermissionViewModel> viewModels = list.ConvertAll(x => new AppPermissionViewModel(x));
            return Ok(viewModels);
        }

        [HttpGet]
        [Route("GetPermissions/{roleId}")]
        [ActionName("GetPermissionsForRole")]
        public async Task<IHttpActionResult> GetPermissionsForRole(string roleId)
        {
            List<ApplicationPermissionViewModel> applicationPermissionsViewModel = new List<ApplicationPermissionViewModel>();

            var permissions = await db.Permissions.Include(x => x.Resource).Where(permission => permission.RoleId == roleId).OrderBy(x => x.Resource.ResourceType).ToListAsync();

            foreach (var permission in permissions)
            {
                applicationPermissionsViewModel.Add(new ApplicationPermissionViewModel(permission.Id, permission.ResourceId, permission.RoleId, permission.IsAllowed));
            }

            return Ok(applicationPermissionsViewModel);
        }
    }
}
