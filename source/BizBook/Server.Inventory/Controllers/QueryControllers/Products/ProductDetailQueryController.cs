using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Products;

using ServiceLibrary.Products;

using Rm = RequestModel.Products.ProductDetailRequestModel;
using Vm = ViewModel.Products.ProductDetailViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Products
{
    [RoutePrefix("api/ProductDetailQuery")]
    public class ProductDetailQueryController : BaseQueryController<ProductDetail, Rm, Vm>
    {

        public ProductDetailQueryController() : base(new ProductDetailService(new BaseRepository<ProductDetail>(BusinessDbContext.Create())))
        {

        }

        [Route("Barcode")]
        [ActionName("Barcode")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBarcode()
        {
            var service = Service as ProductDetailService;
            string barcode = await service.GetBarcode(AppUser.ShopId);
            return Ok(barcode);
        }

        [Route("History")]
        [ActionName("History")]
        [HttpPost]
        public async Task<IHttpActionResult> History(Rm request)
        {
            try
            {

                ProductDetailService service = Service as ProductDetailService;
                var content = await service.GetHistory(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                //telemetryClient.TrackException(exception);
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }

        [Route("HistoryByDate")]
        [ActionName("HistoryByDate")]
        [HttpPost]
        public async Task<IHttpActionResult> GetHistoryByDate(Rm request)
        {
            try
            {
                ProductDetailService service = Service as ProductDetailService;
                var content = await service.GetHistoryByDate(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                //telemetryClient.TrackException(exception);
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }

        //[Route("Report")]
        //[ActionName("Report")]
        //[HttpPost]
        //public async Task<IHttpActionResult> Report(ProductReportRequestModel request)
        //{
        //    try
        //    {
        //        var reportService = new ProductReportService2();
        //        if (request == null)
        //        {
        //            return BadRequest("Request should be not null");
        //        }

        //        var service = Service as ProductDetailService;
        //        var reports = service.GetProductStockReport(request);
                
        //        var response = this.Request.CreateResponse(HttpStatusCode.OK, reports);
        //        return ResponseMessage(response);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        [Route("SearchByWarehouse")]
        [ActionName("SearchByWarehouse")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchByWarehouse(Rm request)
        {
            try
            {
                var service = this.Service as ProductDetailService;
                Tuple<List<Vm>, int> content = await service.SearchByWarehouseAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }
    }
}
