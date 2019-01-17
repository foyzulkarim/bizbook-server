using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Products;
using RequestModel.Products;
using ViewModel.Products;

namespace Server.Inventory.Controllers.CommandControllers.Products
{
    [RoutePrefix("api/ProductCategory")]
    public class ProductCategoryController : BaseCommandController<ProductCategory,ProductCategoryRequestModel,ProductCategoryViewModel>
    {
        public ProductCategoryController() : base(new BaseService<ProductCategory, ProductCategoryRequestModel, ProductCategoryViewModel>(new BaseRepository<ProductCategory>(BusinessDbContext.Create())))
        {
            
        }
    }
}