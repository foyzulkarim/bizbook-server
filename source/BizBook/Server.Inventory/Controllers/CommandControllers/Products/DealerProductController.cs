using Model.Products;
using RequestModel.Products;
using System.Web.Http;
using ViewModel.Products;
using CommonLibrary.Service;
using Model;
using CommonLibrary.Repository;
using ServiceLibrary.Products;

namespace Server.Inventory.Controllers.CommandControllers.Products
{
    using Server.Inventory.Filters;

    [RoutePrefix("api/DealerProduct")]
    public class DealerProductController : BaseCommandController<DealerProduct, DealerProductRequestModel, DealerProductViewModel>
    {
        public DealerProductController() : base(new BaseService<DealerProduct, DealerProductRequestModel, DealerProductViewModel>(new BaseRepository<DealerProduct>(BusinessDbContext.Create())))
        {
        }


        [HttpPut]
        [Route("UpdateDues")]
        [ActionName("UpdateDues")]
        [EntitySaveFilter]
        public IHttpActionResult UpdateDues(DealerProductDetailUpdateModel model)
        {
            DealerProductService service = new DealerProductService(new BaseRepository<DealerProduct>(BusinessDbContext.Create()));
           
            AddCommonValues(model, model.Transaction);

            foreach (var entity in model.DealerProductTransactions)
            {
                AddCommonValues(model, entity);
                entity.TransactionId = model.Transaction.Id;
            }
            
            bool updated = service.UpdateDues(model);
            return Ok(updated);
        }
    }
}
