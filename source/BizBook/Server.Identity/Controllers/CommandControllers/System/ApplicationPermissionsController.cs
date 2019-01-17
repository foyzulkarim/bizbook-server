using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Server.Identity.Models;

namespace Server.Identity.Controllers.CommandControllers.System
{
    [RoutePrefix("api/ApplicationPermissions")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationPermissionsController : ApiController
    {
        private SecurityDbContext db = new SecurityDbContext();

        [HttpPost]
        [Route("Post")]
        [ActionName("Post")]
        public async Task<IHttpActionResult> Post(ApplicationPermission permission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationPermission applicationPermission = db.Permissions.FirstOrDefault(x => x.ResourceId == permission.ResourceId && x.RoleId == permission.RoleId);
            if (applicationPermission == null)
            {
                permission.Id = Guid.NewGuid().ToString();
                db.Permissions.Add(permission);
                await db.SaveChangesAsync();
            }
            else
            {
               ModelState.AddModelError("Duplicate", "This resource is already assigned with this role");
            }

            return Ok(permission.Id);
        }

        [HttpPut]
        [Route("Put")]
        [ActionName("Put")]
        public async Task<IHttpActionResult> Put(ApplicationPermission permission)
        {           
            db.Entry(permission).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok(permission);
        }

        [HttpDelete]
        [Route("Delete")]
        [ActionName("Delete")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            ApplicationPermission resourcePermission = db.Permissions.Find(id);
            if (resourcePermission == null)
            {
                return NotFound();
            }

            db.Permissions.Remove(resourcePermission);
            await db.SaveChangesAsync();

            return Ok(resourcePermission);
        }

        [HttpPost]
        [Route("UpdatePermissionsForRole")]
        [ActionName("UpdatePermissionsForRole")]
        public async Task<IHttpActionResult> UpdatePermissionsForRole(List<ApplicationPermission> updatedApplicationPermissions )
        {
            var resultApplicationPermissions = new List<ApplicationPermissionViewModel>();
                        
            foreach (var applicationPermission in updatedApplicationPermissions)
            {
                var existingApplicationPermission = db.Permissions.Where(
                    permission =>
                        permission.RoleId == applicationPermission.RoleId &&
                        permission.ResourceId == applicationPermission.ResourceId).FirstOrDefault();
                
                if (existingApplicationPermission != null)
                {
                    existingApplicationPermission.IsAllowed = applicationPermission.IsAllowed;
                    existingApplicationPermission.IsDisabled = !existingApplicationPermission.IsAllowed;
                    db.Permissions.Attach(existingApplicationPermission);
                    db.Entry(existingApplicationPermission).State = EntityState.Modified;

                    resultApplicationPermissions.Add(new ApplicationPermissionViewModel(existingApplicationPermission.Id,existingApplicationPermission.ResourceId,existingApplicationPermission.RoleId,existingApplicationPermission.IsAllowed,existingApplicationPermission.IsDisabled));
                }
                else
                {
                    applicationPermission.IsDisabled = false;
                    db.Permissions.Add(applicationPermission);
                    resultApplicationPermissions.Add(new ApplicationPermissionViewModel(applicationPermission.Id, applicationPermission.ResourceId, applicationPermission.RoleId, applicationPermission.IsAllowed, applicationPermission.IsDisabled));
                }
            }

            await db.SaveChangesAsync();

            return Ok(resultApplicationPermissions);
        }
    }
}
