using Model.Products;
using RequestModel.Products;
using System.Web.Http;
using ViewModel.Products;
using CommonLibrary.Service;
using CommonLibrary.Repository;
using Model;

namespace Server.Inventory.Controllers.QueryControllers.Products
{
    [RoutePrefix("api/SupplierProductQuery")]
    public class SupplierProductQueryController : BaseQueryController<SupplierProduct, SupplierProductRequestModel, SupplierProductViewModel>
    {
        public SupplierProductQueryController() : base(new BaseService<SupplierProduct, SupplierProductRequestModel, SupplierProductViewModel>(new BaseRepository<SupplierProduct>(BusinessDbContext.Create())))
        {

        }
    }
}
