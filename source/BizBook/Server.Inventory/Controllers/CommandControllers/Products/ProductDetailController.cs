using System.Web.Http;
using CommonLibrary.Repository;
using Model;
using Model.Products;
using RequestModel.Products;
using ServiceLibrary.Products;
using ViewModel.Products;

namespace Server.Inventory.Controllers.CommandControllers.Products
{
    [RoutePrefix("api/ProductDetail")]
    public class ProductDetailController : BaseCommandController<ProductDetail, ProductDetailRequestModel, ProductDetailViewModel>
    {
        public ProductDetailController() : base(new ProductDetailService(new BaseRepository<ProductDetail>(BusinessDbContext.Create())))
        {

        }
    }
}