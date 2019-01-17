using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using ServiceLibrary.Customers;
using ViewModel.Customers;
using Rm = RequestModel.Customers.CustomerRequestModel;
using M = Model.Customers.Customer;
using Vm = ViewModel.Customers.CustomerViewModel;

namespace Server.Inventory.Controllers.QueryControllers.Customers
{
    [RoutePrefix("api/CustomerQuery")]
    public class CustomerQueryController : BaseQueryController<M, Rm, Vm>
    {
        private readonly CustomerService service;

        public CustomerQueryController() : base(new CustomerService(new BaseRepository<M>(BusinessDbContext.Create())))
        {
            service = (CustomerService)Service;
        }

        [Route("Barcode")]
        [ActionName("Barcode")]
        [HttpGet]
        public IHttpActionResult GetBarcode()
        {
            string barcode = service.GetBarcode(AppUser.ShopId);
            return Ok(barcode);
        }

        [Route("CustomerProductView")]
        [ActionName("CustomerProductView")]
        [HttpPost]
        public async Task<IHttpActionResult> CustomerProductView(Rm request)
        {
            try
            {
                var customerService = Service as CustomerService;
                Tuple<List<CustomerProductViewModel>, int> content = await customerService.GetCustomerProductView(request);
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

