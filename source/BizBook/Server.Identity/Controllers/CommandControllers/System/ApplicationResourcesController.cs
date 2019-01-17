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
    [RoutePrefix("api/ApplicationResources")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationResourcesController : ApiController
    {
        private SecurityDbContext db;
        // GET: api/ApplicationResources/5
        [ResponseType(typeof(ApplicationResource))]
        [HttpGet]
        [Route("GetApplicationResource/{id}")]
        [ActionName("GetApplicationResource")]
        public async Task<IHttpActionResult> GetApplicationResource(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            ApplicationResource applicationResource = await db.Resources.FindAsync(id);
            if (applicationResource == null)
            {
                return NotFound();
            }
            var appResourceViewModel = new AppResourceViewModel(applicationResource);
            return Ok(appResourceViewModel);
        }

        [HttpPut]
        [Route("PutApplicationResource")]
        [ActionName("PutApplicationResource")]
        // PUT: api/ApplicationResources/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutApplicationResource(ApplicationResource applicationResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (applicationResource.Id == null)
            {
                return BadRequest();
            }
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            db.Entry(applicationResource).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationResourceExists(applicationResource.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(applicationResource);
        }

        [HttpPost]
        [Route("PostApplicationResource")]
        [ActionName("PostApplicationResource")]
        // POST: api/ApplicationResources
        [ResponseType(typeof(ApplicationResource))]
        public async Task<IHttpActionResult> PostApplicationResource(ApplicationResource applicationResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             applicationResource.Id = Guid.NewGuid().ToString();
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            db.Resources.Add(applicationResource);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationResourceExists(applicationResource.Id))
                {
                    return Conflict();
                }
                throw;
            }

            return Ok(applicationResource.Id);
        }

        [HttpDelete]
        [Route("DeleteApplicationResource")]
        [ActionName("DeleteApplicationResource")]
        // DELETE: api/ApplicationResources/5
        [ResponseType(typeof(ApplicationResource))]
        public async Task<IHttpActionResult> DeleteApplicationResource(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            ApplicationResource applicationResource = await db.Resources.FindAsync(id);
            if (applicationResource == null)
            {
                return NotFound();
            }

            db.Resources.Remove(applicationResource);
            await db.SaveChangesAsync();

            return Ok(applicationResource);
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

        private bool ApplicationResourceExists(string id)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            return db.Resources.Count(e => e.Id == id) > 0;
        }
    }
}