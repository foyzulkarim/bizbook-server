using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using RequestModel.Shops;
using ViewModel.Shops;
using M = Model.Shops.Supplier;

namespace Server.Inventory.Controllers.CommandControllers.Shops
{
    [RoutePrefix("api/Supplier")]
    public class SupplierController : BaseCommandController<M, SupplierRequestModel, SupplierViewModel>
    {
        public SupplierController() : base(new BaseService<M, SupplierRequestModel, SupplierViewModel>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }
    }
}