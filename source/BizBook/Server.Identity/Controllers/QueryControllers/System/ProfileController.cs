using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.System
{
    [Authorize]
    [RoutePrefix("api/Profile")]
    public class ProfileController : ApiController
    {
        public ApplicationUserManager manager;
        private SecurityDbContext db;

        public ProfileController()
        {
            db = SecurityDbContext.Create();
        }

        [Route("Details")]
        public async Task<IHttpActionResult> Get()
        {
            string id = User.Identity.GetUserId();
            
            ApplicationUser user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            var roles =
       await db.ApplicationUserRoles.Where(userRole => userRole.UserId == user.Id)
                    .Join(db.ApplicationRoles, userRole => userRole.RoleId, role => role.Id, (userRole, role) => role).FirstAsync();

            if (user != null)
            {
                var model = new  
                {
                    Id = id,
                    user.ShopId,
                    user.IsActive,
                    user.FirstName,
                    user.Roles.First().RoleId,
                    RoleName = roles.Name,
                    Phone=user.PhoneNumber, 
                    user.UserName
                };
                return Ok(model);
            }

            return NotFound();
        }

    }
}