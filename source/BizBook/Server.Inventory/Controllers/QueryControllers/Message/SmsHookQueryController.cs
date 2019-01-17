using Model.Message;
using RequestModel.Message;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModel.Message;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.QueryControllers.Message
{
    using System.Threading.Tasks;

    using ServiceLibrary.Messages;

    [RoutePrefix("api/SmsHookQuery")]
    public class SmsHookQueryController : BaseQueryController<SmsHook, SmsHookRequestModel, SmsHookViewModel>
    {
        public SmsHookQueryController() : base(new SmsHookService(new BaseRepository<SmsHook>(BusinessDbContext.Create())))
        {
        }

        [Route("SearchHooks")]
        [ActionName("SearchHooks")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchHooks(SmsHookRequestModel request)
        {
            try
            {
                SmsHookService service = this.Service as SmsHookService;
                var v = await service.SearchHooks(request);
                Tuple<List<SmsHook>, int> content = new Tuple<List<SmsHook>, int>(v, v.Count);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
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
