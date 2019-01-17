using CommonLibrary.Repository;
using Model;
using Model.Warehouses;
using RequestModel.Warehouses;
using ViewModel.Warehouses;
using System.Web.Http;
using ServiceLibrary.Warehouses;

namespace Server.Inventory.Controllers.CommandControllers.Warehouses
{
    [RoutePrefix("api/StockTransfer")]
    public class StockTransferController : BaseCommandController<StockTransfer, StockTransferRequestModel, StockTransferViewModel>
    {
        public StockTransferController(): base(new StockTransferService(new BaseRepository<StockTransfer>(BusinessDbContext.Create())))
        {

        }


        [Route("UpdateState")]
        [ActionName("UpdateState")]
        [HttpPut]
        public IHttpActionResult UpdateState(StockTransfer stockTransfer)
        {
            if (stockTransfer==null || string.IsNullOrWhiteSpace(stockTransfer.Id))
            {
                return BadRequest("Data should not be null");
            }

            var service = Service as StockTransferService;
            bool updateState = service.UpdateState(stockTransfer.Id);
            return Ok(updateState);
        }
    }
}
