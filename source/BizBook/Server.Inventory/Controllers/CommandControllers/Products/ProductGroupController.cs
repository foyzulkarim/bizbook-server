using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using RequestModel.Products;
using ViewModel.Products;
using M=Model.Products.ProductGroup;

namespace Server.Inventory.Controllers.CommandControllers.Products
{
    [RoutePrefix("api/ProductGroup")]
    public class ProductGroupController : BaseCommandController<M, ProductGroupRequestModel,ProductGroupViewModel>
    {
        public ProductGroupController() : base(new BaseService<M, ProductGroupRequestModel, ProductGroupViewModel>(new BaseRepository<M>(BusinessDbContext.Create())))
        {

        }         
    }
}