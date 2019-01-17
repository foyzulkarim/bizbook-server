using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using ServiceLibrary.Shops;
using ViewModel.History;
using ViewModel.Shops;
using Rm = RequestModel.Shops.SupplierRequestModel;
using M = Model.Shops.Supplier;
using Vm = ViewModel.Shops.SupplierViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Shops
{
    [RoutePrefix("api/SupplierQuery")]
  public class SupplierQueryController : BaseQueryController<M, Rm, Vm>
    {
        public SupplierQueryController() : base(new SupplierService(new BaseRepository<M>(BusinessDbContext.Create())))
        {
        }

        [Route("History")]
        [ActionName("History")]
        [HttpPost]
        public async Task<IHttpActionResult> History(Rm request)
        {
            try
            {
                SupplierService service = Service as SupplierService;
                Tuple<List<HistoryViewModel>, int> content = await service.GetHistory(request);
                Vm model = service.GetDetail(request.ParentId);
                var purchaseTotal = content.Item1.Where(x => x.Type == "Purchase").Sum(x => x.Total);
                var paymentTotal = content.Item1.Where(x => x.Type == "Payment").Sum(x => x.Total);
                var responseContent = new
                {
                    Supplier = model,
                    Histories = content.Item1,
                    PurchaseTotal=purchaseTotal,
                    PaymentTotal=paymentTotal
                };
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseContent);
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

       
    }
}
