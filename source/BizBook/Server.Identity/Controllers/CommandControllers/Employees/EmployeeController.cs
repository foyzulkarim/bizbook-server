using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Server.Identity.Models;

namespace Server.Identity.Controllers.CommandControllers.Employees
{
    [RoutePrefix("api/Employee")]
    [Authorize(Roles = "SuperAdmin,ShopAdmin")]
    public class EmployeeController : ApiController
    {
        [HttpPost]
        [Route("Add")]
        [ActionName("Add")]
        public async Task<IHttpActionResult> Add(AppUserViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var db = Request.GetOwinContext().Get<SecurityDbContext>();
            var manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            var userId = User.Identity.GetUserId<string>();
            var role = db.Roles.Find(employee.RoleId);
            var me = manager.FindById(userId);

            var existingUserInfo = manager.FindByEmail(employee.UserName);
            if (existingUserInfo != null)
            {
                return Conflict();
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = employee.UserName,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmailConfirmed = true,
                ShopId = me.ShopId,
                UserName = employee.UserName, RoleName = role.Name
            };

            IdentityResult result = await manager.CreateAsync(user, employee.Password);
            if (result.Succeeded)
            {
                var identityUserRole =
                    db.ApplicationUserRoles.Add(new IdentityUserRole { RoleId = employee.RoleId, UserId = user.Id });
                await db.SaveChangesAsync();
            }

            return Ok(user.Id);
        }

        // edit , delete goes below
        [HttpPut]
        [Route("Edit")]
        [ActionName("UpdateEmployeeInfo")]
        public async Task<IHttpActionResult> UpdateEmployeeInfo(AppUserViewModel employeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var db = Request.GetOwinContext().Get<SecurityDbContext>();
            var manager = Request.GetOwinContext().Get<ApplicationUserManager>();


            var employeeDetail = db.Users.Find(employeeInfo.Id);

            if (employeeDetail == null)
            {
                return Conflict();
            }

            employeeDetail.IsActive = employeeInfo.IsActive;
            employeeDetail.PhoneNumber = employeeInfo.PhoneNumber;
            employeeDetail.Email = employeeInfo.Email;
            employeeDetail.FirstName = employeeInfo.FirstName;
            employeeDetail.LastName = employeeInfo.LastName;
            employeeDetail.PhoneNumber = employeeInfo.PhoneNumber;
            employeeDetail.ShopId = employeeInfo.ShopId;
            employeeDetail.UserName = employeeInfo.Email;

            var result = await manager.UpdateAsync(employeeDetail);
            IdentityUserRole entity = db.ApplicationUserRoles.FirstOrDefault(x => x.UserId == employeeDetail.Id);
            db.ApplicationUserRoles.Remove(entity);
            db.SaveChanges();
            var identityUserRole = db.ApplicationUserRoles.Add(new IdentityUserRole { RoleId = employeeInfo.RoleId, UserId = employeeDetail.Id });
            await db.SaveChangesAsync();
            return Ok(result);
        }
    }
}
