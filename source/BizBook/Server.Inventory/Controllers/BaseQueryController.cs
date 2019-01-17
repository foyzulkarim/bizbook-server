using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Model;
using CommonLibrary.RequestModel;
using CommonLibrary.Service;
using CommonLibrary.ViewModel;
using Model;
using Serilog;
using Server.Inventory.Attributes;
using Server.Inventory.Filters;
using Server.Inventory.Models;

namespace Server.Inventory.Controllers
{
    using System.Linq;

    [BizBookAuthorization]
    [ShopChildQuery]
    public class BaseQueryController<T, TRm, TVm> : ApiController where T : Entity where TVm : BaseViewModel<T> where TRm : RequestModel<T>
    {
        public static ILogger Logger = Log.ForContext(typeof(BaseQueryController<T, TRm, TVm>));

        public ApplicationUser AppUser;
        private BusinessDbContext Db;
        protected BaseService<T, TRm, TVm> Service;
        protected string typeName = "";

        public BaseQueryController(BaseService<T, TRm, TVm> service)
        {
            Service = service;
            typeName = typeof(T).Name;
        }

        [Route("Search")]
        [ActionName("Search")]
        [HttpPost]
        public async Task<IHttpActionResult> Search(TRm request)
        {
            try
            {
                Tuple<List<TVm>, int> content = await Service.SearchAsync(request);
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

        [Route("Dropdown")]
        [ActionName("Dropdown")]
        [HttpPost]
        public async Task<IHttpActionResult> Dropdown(TRm request)
        {
            try
            {
                var content = await Service.GetDropdownListAsync(request);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while trying to get Dropdown {TypeName} with Request {Request}", typeName, request);
                return InternalServerError(exception);
            }
        }

        [Route("Detail")]
        [ActionName("Detail")]
        public IHttpActionResult Detail(string id)
        {
            try
            {
                TVm content = Service.GetDetail(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, content);
                return ResponseMessage(response);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while trying to get Detail {TypeName} with Request {Id}", typeName, id);
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("SearchDetail")]
        [ActionName("SearchDetail")]
        public async Task<IHttpActionResult> SearchDetail(TRm request)
        {
            try
            {
                request.IsIncludeParents = true;
                Tuple<List<TVm>, int> content = await Service.SearchAsync(request);
                TVm vm = content.Item1.FirstOrDefault();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, vm);
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