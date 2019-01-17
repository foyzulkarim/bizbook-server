using CommonLibrary.Repository;
using Model;
using Model.Warehouses;
using RequestModel.Warehouses;
using ViewModel.Warehouses;
using System.Web.Http;
using ServiceLibrary.Warehouses;

namespace Server.Inventory.Controllers.QueryControllers.Warehouses
{
    [RoutePrefix("api/StockTransferQuery")]
    public class StockTransferQueryController : BaseQueryController<StockTransfer, StockTransferRequestModel, StockTransferViewModel>
    {
        public StockTransferQueryController() : base(new StockTransferService(new BaseRepository<StockTransfer>(BusinessDbContext.Create())))
        {

        }
    }
}
