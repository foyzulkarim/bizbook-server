using Model.Products;
using RequestModel.Products;
using System.Web.Http;
using ViewModel.Products;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;
using Server.Inventory.Filters;
using ServiceLibrary.Products;

namespace Server.Inventory.Controllers.CommandControllers.Products
{
    [RoutePrefix("api/SupplierProduct")]
    public class SupplierProductController : BaseCommandController<SupplierProduct, SupplierProductRequestModel, SupplierProductViewModel>
    {
        public SupplierProductController() : base(new BaseService<SupplierProduct, SupplierProductRequestModel, SupplierProductViewModel>(new  BaseRepository<SupplierProduct>(BusinessDbContext.Create())))
        {

        }

        [HttpPut]
        [Route("UpdateDues")]
        [ActionName("UpdateDues")]
        [EntitySaveFilter]
        public IHttpActionResult UpdateDues(SupplierProductDetailUpdateModel model)
        {
            var service = new SupplierProductService(new BaseRepository<SupplierProduct>(BusinessDbContext.Create()));

            AddCommonValues(model, model.Transaction);

            foreach (var entity in model.SupplierProductTransactions)
            {
                AddCommonValues(model, entity);
                entity.TransactionId = model.Transaction.Id;
            }

            bool updated = service.UpdateDues(model);
            return Ok(updated);
        }
    }


}
