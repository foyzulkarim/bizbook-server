using CommonLibrary.ViewModel;
using Model.Products;

namespace ViewModel.Products
{
    public class ProductCategoryViewModel : BaseViewModel<ProductCategory>
    {
        public ProductCategoryViewModel(ProductCategory x) : base(x)
        {
            Name = x.Name;
            ProductGroupId = x.ProductGroupId;
            if (x.ProductGroup != null)
            {
                ProductGroupName = x.ProductGroup.Name;
            }

            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }

        [IsViewable] public string ProductGroupName { get; set; }

        [IsViewable] public string Name { get; set; }
        public string ProductGroupId { get; set; }
    }
}