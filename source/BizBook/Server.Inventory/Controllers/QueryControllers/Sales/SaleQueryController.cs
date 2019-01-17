using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Shops;

using RequestModel.Shops;

using ViewModel.Shops;
using Service = ServiceLibrary.Sales.SaleService;
using Rm = RequestModel.Sales.SaleRequestModel;
using M = Model.Sales.Sale;
using Vm = ViewModel.Sales.SaleViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Sales
{
    using Model.Customers;
    using Model.Dealers;
    using RequestModel.Reports;
    using RequestModel.Sales;

    using ServiceLibrary.Customers;
    using ServiceLibrary.Sales;

    using ViewModel.History;

    [RoutePrefix("api/SaleQuery")]
    public class SaleQueryController : BaseQueryController<M, Rm, Vm>
    {

        public SaleQueryController() : base(new Service(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }

        [ActionName("SearchDelivery")]
        [Route("SearchDelivery")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchDelivery()
        {
            //var db = Request.GetOwinContext().Get<SecurityDbContext>();
            try
            {
                var service = Service as Service;
                Tuple<List<Vm>, int> content = service.SearchDelivery(AppUser.Id, AppUser.ShopId);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request SearchDelivery", typeName);
                return InternalServerError(exception);
            }
        }

        [ActionName("SearchReadyToDeparture")]
        [Route("SearchReadyToDeparture")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchReadyToDeparture(CourierOrderRequestModel request)
        {
            //var db = Request.GetOwinContext().Get<SecurityDbContext>();
            try
            {
                var service = Service as Service;
                var content = await service.SearchReadyToDeparture(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request SearchDelivery", typeName);
                return InternalServerError(exception);
            }
        }



        [Route("Receipt")]
        [ActionName("Receipt")]
        [HttpPost]
        public IHttpActionResult Receipt(string id)
        {
            var saleViewModel = Service.GetDetail(id);
            var shopRepository = new BaseRepository<Shop>(BusinessDbContext.Create());
            var shopService = new BaseService<Shop, ShopRequestModel, ShopViewModel>(shopRepository);
            saleViewModel.Shop = shopService.GetDetail(AppUser.ShopId);
            var response = Request.CreateResponse(HttpStatusCode.OK, saleViewModel);
            return ResponseMessage(response);
        }

        //[Route("Report")]
        //[ActionName("Report")]
        //[HttpPost]
        //public async Task<IHttpActionResult> Report(SaleReportRequestModel request)
        //{
        //    try
        //    {
        //        if (request == null)
        //        {
        //            return this.BadRequest();
        //        }

        //        SaleReportService reportService = new SaleReportService();
        //        Tuple<List<SaleReportViewModel>, int> saleReports = await reportService.SearchAsync(request);
        //        var response = this.Request.CreateResponse(HttpStatusCode.OK, saleReports.Item1);
        //        return ResponseMessage(response);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        [Route("OrderNumber")]
        [ActionName("OrderNumber")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOrderNumber()
        {
            var service = Service as SaleService;
            string barcode = await service.GetOrderNumber(AppUser.ShopId);
            return Ok(barcode);
        }

        [Route("BuyerHistory")]
        [ActionName("BuyerHistory")]
        [HttpPost]
        public async Task<IHttpActionResult> BuyerHistory(Rm request)
        {
            try
            {
                var saleService = Service as SaleService;
                object model = null;
                Tuple<List<HistoryViewModel>, int> content = await saleService.GetBuyerHistory(request);
                if (request.IsDealerSale)
                {
                    var dealerRepository = new BaseRepository<Dealer>(BusinessDbContext.Create());
                    model = dealerRepository.GetById(request.ParentId);
                }
                else
                {
                    var customerService = new CustomerService(new BaseRepository<Customer>(BusinessDbContext.Create()));
                    model = customerService.GetDetail(request.ParentId);
                }


                var purchaseTotal = content.Item1.Where(x => x.Type == "Sale").Sum(x => x.Total);

                var paymentTotal = content.Item1.Where(x => x.Type == "Payment").Sum(x => x.Total);

                var responseContent = new
                {
                    Customer = model,
                    Histories = content.Item1,
                    SaleTotal = purchaseTotal,
                    PaymentTotal = paymentTotal,
                    BalanceTotal = purchaseTotal - paymentTotal
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseContent);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }

        [Route("ProductHistory")]
        [ActionName("ProductHistory")]
        [HttpPost]
        public async Task<IHttpActionResult> ProductHistory(Rm request)
        {
            try
            {
                var saleService = Service as SaleService;
                Tuple<List<HistoryViewModel>, int> content = await saleService.GetProductHistory(request);
                var responseContent = new { Histories = content.Item1 };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseContent);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }

        [Route("PendingProducts")]
        [ActionName("PendingProducts")]
        [HttpPost]
        public async Task<IHttpActionResult> PendingProducts(Rm request)
        {
            try
            {
                //SaleRequestModel request = new SaleRequestModel("");
                //request.OrderState = OrderState.Pending.ToString();
                //request.Page = -1;
                //request.IsIncludeParents = true;
                var saleService = Service as SaleService;
                Tuple<List<Vm>, List<HistoryViewModel>> content = await saleService.GetPendingProducts(request);

                var responseContent = new { Sales = content.Item1, Histories = content.Item2 };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseContent);
                response.Headers.Add("Count", content.Item2.ToString());
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while Searching {TypeName} with Request", typeName);
                return InternalServerError(exception);
            }
        }

        [Route("DailySalesOverview")]
        [ActionName("DailySalesOverview")]
        [HttpPost]
        public async Task<IHttpActionResult> DailySalesOverview(DailySalesOverviewRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.DailySalesOverviewAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while DailySalesOverview with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("MonthlySalesOverview")]
        [ActionName("MonthlySalesOverview")]
        [HttpPost]
        public async Task<IHttpActionResult> MonthlySalesOverview(DailySalesOverviewRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.MonthlySalesOverviewAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while DailySalesOverview with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("yearlySalesOverview")]
        [ActionName("YearlySalesOverview")]
        [HttpPost]
        public async Task<IHttpActionResult> YearlySalesOverview(DailySalesOverviewRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.YearlySalesOverviewAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while DailySalesOverview with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("CustomerSearchBySale")]
        [ActionName("CustomerSearchBySale")]
        [HttpPost]
        public async Task<IHttpActionResult> CustomerSearchBySale(CustomerBySaleRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.CustomerSearchBySale(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while CustomerSearchBySale with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("SalesByProductDetail")]
        [ActionName("SalesByProductDetail")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesByProductDetail(SalesByProductRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.SalesByProductDetail(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while SalesByProduct with request ");
                return this.InternalServerError(exception);
            }
        }

        //SalesByProductCategory
        [Route("SalesByProductCategory")]
        [ActionName("SalesByProductCategory")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesByProductCategory(SalesByProductRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.SalesByProductCategory(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while SalesByProductCategory with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("SalesByProductGroup")]
        [ActionName("SalesByProductGroup")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesByProductGroup(SalesByProductRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.SalesByProductGroup(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while SalesByProductGroup with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("DeliveredProductCategories")]
        [ActionName("DeliveredProductCategories")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveredProductCategories(Rm request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.DeliveredProductCategories(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while DeliveredProductCategories with request ");
                return this.InternalServerError(exception);
            }
        }

        [Route("SalesByZone")]
        [ActionName("SalesByZone")]
        [HttpPost]
        public async Task<IHttpActionResult> SalesByZone(SaleRequestModel request)
        {
            SaleService service = this.Service as SaleService;
            try
            {
                request.ShopId = AppUser.ShopId;
                var list = await service.ZoneWiseSalesReport(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while SalesByProductGroup with request ");
                return this.InternalServerError(exception);
            }
        }
    }
}
