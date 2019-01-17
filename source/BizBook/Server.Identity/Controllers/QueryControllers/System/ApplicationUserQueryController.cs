using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [RoutePrefix("api/ApplicationUserQuery")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationUserQueryController : ApiController
    {
        public ApplicationUserManager manager;
        private SecurityDbContext db = SecurityDbContext.Create();

        public ApplicationUserQueryController()
        {
             manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        [Route("Data")]
        [ActionName("Data")]
        [HttpGet]
        public async Task<IHttpActionResult> Data()
        {           
            IQueryable<ApplicationUser> queryable = manager.Users;
            var roles = db.Roles.ToDictionary(x => x.Id, x => x.Name);

            var listAsync = await queryable.ToListAsync();            
            var viewModels = listAsync.Select(x => new AppUserViewModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                RoleId = x.Roles.First().RoleId,
                RoleName = roles[x.Roles.First().RoleId],
                PhoneNumber = x.PhoneNumber,
                IsActive = x.IsActive,
                Id = x.Id,
                ShopId = x.ShopId
            }).ToList();

            return Ok(viewModels);
        }
    }
}
