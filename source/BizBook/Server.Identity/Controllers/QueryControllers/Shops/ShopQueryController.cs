using Rm = RequestModel.Shops.ShopRequestModel;
using M = Model.Shops.Shop;

namespace Server.Identity.Controllers.QueryControllers.Shops
{
    using CommonLibrary.Repository;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Net;
    using global::System.Net.Http;
    using global::System.Threading.Tasks;
    using global::System.Web.Http;

    using Model;

    using Serilog;
    using ServiceLibrary.Shops;

    using ViewModel.Shops;

    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/ShopQuery")]
    public class ShopQueryController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(ShopQueryController));

        ShopService shopService;

        public ShopQueryController()
        {
            shopService = new ShopService(new BaseRepository<M>(BusinessDbContext.Create()));
        }

        [Route("Search")]
        [ActionName("Search")]
        [HttpPost]
        public async Task<IHttpActionResult> Search(Rm request)
        {
            try
            {
                Tuple<List<ShopSuperAdminViewModel>, int> content = await shopService.SearchAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeof(ShopQueryController).ToString(), request);
                return InternalServerError(exception);
            }
        }

        [Route("Dropdown")]
        [ActionName("Dropdown")]
        [HttpPost]
        public async Task<IHttpActionResult> Dropdown(Rm request)
        {
            try
            {
                var content = await shopService.GetDropdownListAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(
                    exception,
                    "Exception occurred while trying to get Dropdown {TypeName} with Request {Request}",
                    typeof(ShopQueryController).ToString(),
                    request);
                return InternalServerError(exception);
            }
        }

        [Route("Detail")]
        [ActionName("Detail")]
        public IHttpActionResult Detail(string id)
        {
            try
            {
                var content = shopService.GetDetail(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while trying to get Detail {TypeName} with Request {Id}", typeof(ShopQueryController).ToString(), id);
                return InternalServerError(exception);
            }
        }
    }
}