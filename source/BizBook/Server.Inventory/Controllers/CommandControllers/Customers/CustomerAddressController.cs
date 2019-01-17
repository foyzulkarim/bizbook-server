using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Customers;
using RequestModel.Customers;
using ViewModel.Customers;

namespace Server.Inventory.Controllers.CommandControllers.Customers
{
    [RoutePrefix("api/CustomerAddress")]
    public class CustomerAddressController : BaseCommandController<Address,AddressRequestModel,AddressViewModel>
    {
        public CustomerAddressController() : base(new BaseService<Address, AddressRequestModel, AddressViewModel>(new BaseRepository<Address>(BusinessDbContext.Create())))
        {
        }
    }
}
