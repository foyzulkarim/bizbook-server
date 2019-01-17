using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using Server.Identity.Models;

namespace Server.Identity.Controllers.CommandControllers.System
{
    [RoutePrefix("api/ApplicationRoles")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationRolesController : ApiController
    {
        private SecurityDbContext db;
        [HttpGet]
        [Route("GetApplicationRole/{id}")]
        [ActionName("GetApplicationRole")]
        // GET: api/ApplicationRoles/5
        [ResponseType(typeof(ApplicationRole))]
        public async Task<IHttpActionResult> GetApplicationRole(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();

            ApplicationRole applicationRole = await db.ApplicationRoles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return Ok(applicationRole);
        }

        [HttpPut]
        [Route("PutApplicationRole")]
        [ActionName("PutApplicationRole")]
        // PUT: api/ApplicationRoles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutApplicationRole(ApplicationRole applicationRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db = Request.GetOwinContext().Get<SecurityDbContext>();

            db.Entry(applicationRole).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationRoleExists(applicationRole.Id))
                {
                    return NotFound();
                }
                throw;
            }

           return Ok(applicationRole);
        }

        [HttpPost]
        [Route("PostApplicationRole")]
        [ActionName("PostApplicationRole")]
        // POST: api/ApplicationRoles
        [ResponseType(typeof(ApplicationRole))]
        public async Task<IHttpActionResult> PostApplicationRole(ApplicationRole applicationRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            applicationRole.Id = Guid.NewGuid().ToString();
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            db.ApplicationRoles.Add(applicationRole);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationRoleExists(applicationRole.Id))
                {
                    return Conflict();
                }
                throw;
            }
            return Ok(applicationRole.Id);
          //  return CreatedAtRoute("DefaultApi", new { id = applicationRole.Id }, applicationRole);
        }

        [HttpDelete]
        [Route("DeleteApplicationRole")]
        [ActionName("DeleteApplicationRole")]
        // DELETE: api/ApplicationRoles/5
        [ResponseType(typeof(ApplicationRole))]
        public async Task<IHttpActionResult> DeleteApplicationRole(string id)
        {
            ApplicationRole applicationRole = await db.ApplicationRoles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            db.ApplicationRoles.Remove(applicationRole);
            await db.SaveChangesAsync();

            return Ok(applicationRole);
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

        private bool ApplicationRoleExists(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            return db.ApplicationRoles.Count(e => e.Id == id) > 0;
        }
    }
}