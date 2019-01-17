using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace Server.Identity.Controllers
{
    using Serilog;

    using Server.Identity.Hubs;

    [AllowAnonymous]
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [ActionName("GetValue")]
        [Route("GetValue")]
        public IEnumerable<string> Get()
        {
            //var dbContext = Request.GetOwinContext().Get<BusinessDbContext>();
            //int count = dbContext.Shops.Count();
            Trace.WriteLine("Get request at ValuesController at " + DateTime.UtcNow);
            Log.Logger.Information("hello world at " + DateTime.Now);
            return new[] { " " + DateTime.UtcNow };
        }

        [HttpGet]
        [ActionName("GetConnections")]
        [Route("GetConnections")]
        public string GetConnections()
        {
            string value = RedisService.GetValue("Connections");
            return value;
        }
    }
}
