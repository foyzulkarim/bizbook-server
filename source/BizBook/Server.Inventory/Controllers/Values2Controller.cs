using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using Server.Inventory.Attributes;
using Server.Inventory.Models;

namespace Server.Inventory.Controllers
{
    using Model.Products;

    using Serilog;

    using Server.Inventory.Helpers;

    [RoutePrefix("api/inventory/values")]
    public class Values2Controller : ApiController
    {
        public ApplicationUser AppUser;

        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        [ActionName("GetValue")]
        [Route("GetValue")]
        public IEnumerable<string> Get()
        {
            var time = DateTime.Now;
            ProductGroup group=new ProductGroup();
            group.Name = "test-"+DateTime.Now.Ticks;
            group.Created=DateTime.Now;
            group.Modified=DateTime.Now;
            Log.Logger.Debug("Product Group {@Group}", group);
            Log.Logger.Debug("Inventory values invoked at {@Time}", time);
            return new[] { " " + DateTime.Now };
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("GetById")]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            throw new Exception("interesting message " + DateTime.Now);
        }

        [BizBookAuthorization]
        [HttpGet]
        [ActionName("GetSecuredValue")]
        [Route("GetSecuredValue")]
        public IEnumerable<string> GetSecured()
        {
            return new[] { " " + DateTime.Now, JsonConvert.SerializeObject(AppUser) };
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
