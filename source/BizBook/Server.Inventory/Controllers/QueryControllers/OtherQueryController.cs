using System;
using System.Web.Http;
using CommonLibrary.Model;
using CommonLibrary.RequestModel;
using Serilog;

namespace Server.Inventory.Controllers.QueryControllers
{
    public class OtherQueryController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(OtherQueryController));

        [Route("api/OtherQuery/Dropdown")]
        [ActionName("Dropdown")]
        [HttpPost]
        public IHttpActionResult Dropdown(RequestModel<Entity> request)
        {
            throw new NotImplementedException();
            //try
            //{
            //    List<DropdownViewModel> item1 = new List<DropdownViewModel>
            //    {
            //        new DropdownViewModel {Id = "other", Text = "Other"}
            //    };

            //    int item2 = item1.Count;
            //    var tuple = new Tuple<List<DropdownViewModel>, int>(item1, item2);
            //    var response = Request.CreateResponse(HttpStatusCode.OK, tuple);
            //    return ResponseMessage(response);
            //}
            //catch (Exception exception)
            //{
            //    Logger.Fatal(exception, "Exception occurred while trying to get Dropdown {TypeName} with Request {Request}", "Other", request);
            //    return InternalServerError(exception);
            //}
        }

    }
}