using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Sales;

using Server.Inventory.Attributes;
using Server.Inventory.Models;

using ServiceLibrary.Sales;

namespace Server.Inventory.Controllers.QueryControllers
{
    [BizBookAuthorization]
    [RoutePrefix("api/DashboardQuery")]
    public class DashboardQueryController : ApiController
    {
        public ApplicationUser AppUser;

        [HttpGet]
        [Route("Data")]
        [ActionName("Data")]
        public async Task<IHttpActionResult> Data()
        {
            // sales
            // pending orders
            // total sale
            // top selling product
            // low stock
            //var db = this.Request.GetOwinContext().Get<SecurityDbContext>();

            string shopId = AppUser.ShopId;

            SaleService saleService=new SaleService(new BaseRepository<Sale>(BusinessDbContext.Create()));
            dynamic sales = await saleService.GetSalesAmounts(shopId);

            //var pendingOrders = await saleService.SearchAsync(request);
            //var pendingOrders = await saleService.GetPendingOrders(shopId);
            //dynamic orders = new
            //{
            //    Pending = pendingOrders
            //};

            //ProductDetailService productDetailService=new ProductDetailService(new BaseRepository<ProductDetail>(BusinessDbContext.Create()));
            //ProductDetailRequestModel request1=new ProductDetailRequestModel("");
            //request1.OnHand = 10;
            //request1.ShopId = shopId;

            //var lowStockProducts = await productDetailService.SearchAsync(request1);
            //var products = new
            //{
            //    LowStock=lowStockProducts.Item1,
            //    LowStockCount=lowStockProducts.Item2
            //};

            var data = new
            {
                Sales = sales,
                //Orders = orders,
                //Products = products
            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, data);
            return ResponseMessage(response);
        }
    }
}