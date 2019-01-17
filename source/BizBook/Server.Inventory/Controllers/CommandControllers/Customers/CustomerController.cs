using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Customers;
using RequestModel.Customers;
using ServiceLibrary.Customers;
using ViewModel.Customers;

namespace Server.Inventory.Controllers.CommandControllers.Customers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseCommandController<Customer,CustomerRequestModel,CustomerViewModel>
    {
        public CustomerController() : base(new CustomerService(new BaseRepository<Customer>(BusinessDbContext.Create())))
        {

        }
    }
}