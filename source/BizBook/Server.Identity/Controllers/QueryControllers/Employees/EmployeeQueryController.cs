using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Serilog;
using Server.Identity.Models;

namespace Server.Identity.Controllers.QueryControllers.Employees
{
    [RoutePrefix("api/EmployeeQuery")]
    [Authorize(Roles = "SuperAdmin,ShopAdmin")]
    public class EmployeeQueryController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(EmployeeQueryController));

        private SecurityDbContext db;
        ApplicationUser me;
        ApplicationUserManager manager;

        [Route("Search")]
        [ActionName("Search")]
        [HttpPost]
        public async Task<IHttpActionResult> Search(EmployeeRequestModel request)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            var id = User.Identity.GetUserId();
            me = manager.FindById(id);

            try
            {
                IdentityRole role = null;
                var users =
                    db.Users.AsQueryable().Where(x => x.ShopId == me.ShopId);

                if (!string.IsNullOrWhiteSpace(request.Role))
                {
                    role = db.Roles.FirstOrDefault(x => x.Name == request.Role);
                    if (role != null)
                    {
                        users = users.Where(x => x.Roles.Any(y => y.RoleId == role.Id));
                    }
                }

                // apply the filters here 
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    request.Keyword = request.Keyword.ToLower();
                    users = users.Where(
                        x => x.FirstName.ToLower().Contains(request.Keyword)
                             || x.LastName.ToLower().Contains(request.Keyword)
                             || x.PhoneNumber.ToLower().Contains(request.Keyword));
                }

                users = users.OrderBy(x=>x.FirstName);
                var content = await users.Select(x => new AppUserViewModel
                {
                    FirstName = x.FirstName ?? String.Empty,
                    LastName = x.LastName ?? String.Empty,
                    UserName = x.UserName,
                    Email = x.Email,
                    // RoleId = x.Roles.First().RoleId,
                    // RoleName = roles[x.Roles.First().RoleId],
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,
                    Id = x.Id,
                    ShopId = x.ShopId
                }).ToListAsync();

                Tuple<List<AppUserViewModel>, int> result = new Tuple<List<AppUserViewModel>, int>(content, content.Count);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                response.Headers.Add("Count", content.Count.ToString());

                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception,
                    "Exception occurred while Searching EmployeeQueryControllerwith Request {Request}", request);
                return InternalServerError(exception);
            }
        }

        // implement get by id here 

        [Route("GetEmployeeDetail/{applicationUserId}")]
        [HttpGet]
        [ActionName("GetById")]
        public async Task<IHttpActionResult> GetById(string applicationUserId)
        {
            db = Request.GetOwinContext().Get<SecurityDbContext>();
            manager = Request.GetOwinContext().Get<ApplicationUserManager>();
            var id = User.Identity.GetUserId();
            me = manager.FindById(id);

            try
            {
                var users = db.Users.Include(x => x.Roles);//.Find(applicationUserId);
                var user = users.FirstOrDefault(
                    x => x.Id.Equals(applicationUserId, StringComparison.InvariantCultureIgnoreCase));
                if (user == null)
                {
                    return Conflict();
                }

                // apply the filters here 
                //var content = await users.ToListAsync();
                //Tuple<List<ApplicationUser>, int> result = new Tuple<List<ApplicationUser>, int>(content, content.Count);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, user);
                //response.Headers.Add("Count", content.Count.ToString());

                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception,
                    "Exception occurred while Searching EmployeeQueryControllerwith GetById");
                return InternalServerError(exception);
            }
        }
    }
}
