using System.Web.Http;

namespace Server.Inventory.Controllers.CommandControllers.Warehouses
{
    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Warehouses;
    using RequestModel.Warehouses;
    using ViewModel.Warehouses;

    [RoutePrefix("api/Warehouse")]
    public class WarehouseController : BaseCommandController<Warehouse, WarehouseRequestModel, WarehouseViewModel>
    {
        public WarehouseController(): base(new BaseService<Warehouse, WarehouseRequestModel, WarehouseViewModel>(
        new BaseRepository<Warehouse>(BusinessDbContext.Create())))
        {
            
        }
    }
}
