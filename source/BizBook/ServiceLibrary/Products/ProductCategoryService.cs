using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model.Products;
using RequestModel.Products;
using ViewModel.Products;

namespace ServiceLibrary.Products
{
    
    public class ProductCategoryService : BaseService<ProductCategory, ProductCategoryRequestModel, ProductCategoryViewModel>
    {
        public ProductCategoryService(BaseRepository<ProductCategory> repository) : base(repository)
        {
        }        
    }
}
