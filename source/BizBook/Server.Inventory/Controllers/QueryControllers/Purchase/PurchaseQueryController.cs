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
using ViewModel.Purchases;
using Service = ServiceLibrary.Purchases.PurchaseService;
using Rm = RequestModel.Purchases.PurchaseRequestModel;
using M = Model.Purchases.Purchase;
using Vm = ViewModel.Purchases.PurchaseViewModel;
using ServiceLibrary.Purchases;
using ViewModel.History;

namespace Server.Inventory.Controllers.QueryControllers.Purchase
{
    [RoutePrefix("api/PurchaseQuery")]
    public class PurchaseQueryController : BaseQueryController<M, Rm, Vm>
    {
        public PurchaseQueryController() : base(new Service(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }

       
        [Route("ProductHistory")]
        [ActionName("ProductHistory")]
        [HttpPost]
        public async Task<IHttpActionResult> ProductHistory(Rm request)
        {
            try
            {
                var purchaseService = Service as PurchaseService;
                Tuple<List<HistoryViewModel>, int> content = await purchaseService.GetProductHistoryAsync(request);
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
    }
}
