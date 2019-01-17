using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [RoutePrefix("api/ApplicationResourceQuery")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationResourceQueryController : ApiController
    {
        private SecurityDbContext db = new SecurityDbContext();

        [Route("ResourceDropdown")]
        [ActionName("ResourceDropdown")]
        [HttpGet]
        public async Task<IHttpActionResult> GetApplicationResourceDropdown()
        {
            var roles = await db.Resources.Select(x => new { x.Id, x.Name }).ToListAsync();
            return Ok(roles);
        }

        [Route("Data")]
        [ActionName("Data")]
        [HttpGet]
        public async Task<IHttpActionResult> Data()
        {
            IQueryable<ApplicationResource> queryable = db.Resources.OrderBy(x=>x.ResourceType);
            List<ApplicationResource> resources = await queryable.ToListAsync();
            List<AppResourceViewModel> list = resources.ConvertAll(x => new AppResourceViewModel(x));
            return Ok(list);
        }
    }
}
