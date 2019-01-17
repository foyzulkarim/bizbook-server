using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Serilog;
using Server.Inventory.Attributes;
using Server.Inventory.Filters;
using Server.Inventory.Models;
using ServiceLibrary.Shops;
using Rm = RequestModel.Shops.ShopRequestModel;
using M = Model.Shops.Shop;

namespace Server.Inventory.Controllers.QueryControllers.Shops
{
    [BizBookAuthorization]
    [ShopChildQuery]
    [RoutePrefix("api/MyShopQuery")]
    public class MyShopQueryController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(MyShopQueryController));
        public ApplicationUser AppUser;
        protected ShopService Service;

        public MyShopQueryController()
        {
            Service = new ShopService(new BaseRepository<M>(BusinessDbContext.Create()));
        }

        [Route("Detail")]
        [ActionName("Detail")]
        [HttpPost]
        public IHttpActionResult MyShopDetail()
        {
            var id = AppUser.ShopId;

            try
            {
                var content = Service.GetMyDetail(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while trying to get Detail {TypeName} with Request {Id}",
                    typeof(MyShopQueryController).ToString(), id);
                return InternalServerError(exception);
            }
        }
    }
}