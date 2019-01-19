using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Serilog;
using Server.Identity.Models;

namespace Server.Identity.Controllers.CommandControllers.System
{
    [RoutePrefix("api/ApplicationUsers")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationUsersController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(ApplicationUsersController));

        private SecurityDbContext db;

        public ApplicationUserManager manager;

        //public ApplicationUsersController()
        //{
        //    db = Request.GetOwinContext().Get<SecurityDbContext>();
        //    manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        //}

        [HttpGet]
        [Route("GetApplicationUser/{id}")]
        [ActionName("GetApplicationUser")]
        // GET: api/ApplicationUsers/5
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> GetApplicationUser(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            ApplicationUser applicationUser = await manager.FindByIdAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        [HttpPut]
        [Route("PutApplicationUser")]
        [ActionName("PutApplicationUser")]
        // PUT: api/ApplicationUsers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutApplicationUser(AppUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (viewModel.Id == null)
            {
                return BadRequest();
            }
            // IdentityResult result = await manager.UpdateAsync(applicationUser);
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>(); ApplicationUser user = manager.FindByEmail(viewModel.Email);

            user.Id = viewModel.Id;
            user.Email = viewModel.Email;
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.IsActive = true;
            user.EmailConfirmed = true;
            user.PhoneNumber = viewModel.PhoneNumber;
            user.ShopId = viewModel.ShopId;
            user.UserName = viewModel.Email;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                IdentityUserRole entity = db.ApplicationUserRoles.FirstOrDefault(x => x.UserId == user.Id);
                db.ApplicationUserRoles.Remove(entity);
                db.SaveChanges();
                var identityUserRole = db.ApplicationUserRoles.Add(new IdentityUserRole { RoleId = viewModel.RoleId, UserId = user.Id });
                int i = await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(user.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("PostApplicationUser")]
        [ActionName("PostApplicationUser")]
        // POST: api/ApplicationUsers
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> PostApplicationUser(AppUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            ApplicationUser user = manager.FindByEmail(viewModel.Email);
            if (user != null)
            {
                return Conflict();
            }

            user = new ApplicationUser
            {
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumber = viewModel.PhoneNumber,
                ShopId = viewModel.ShopId,
                UserName = viewModel.Email
            };

            IdentityResult result = await manager.CreateAsync(user, "MyPass@123");
            if (result.Succeeded)
            {
                var identityUserRole = db.ApplicationUserRoles.Add(new IdentityUserRole { RoleId = viewModel.RoleId, UserId = user.Id });
                await db.SaveChangesAsync();
            }

            return Ok(user.Id);
        }

        [HttpDelete]
        [Route("DeleteApplicationUser")]
        [ActionName("DeleteApplicationUser")]
        // DELETE: api/ApplicationUsers/5
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> DeleteApplicationUser(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            db.Users.Remove(applicationUser);
            await db.SaveChangesAsync();

            return Ok(applicationUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db = Request.GetOwinContext().Get<SecurityDbContext>();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}