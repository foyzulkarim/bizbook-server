using CommonLibrary.ViewModel;
using Model.Products;

namespace ViewModel.Products
{
    public class ProductGroupViewModel : BaseViewModel<ProductGroup>
    {
        public ProductGroupViewModel(ProductGroup x) : base(x)
        {
            Id = x.Id;
            Name = x.Name;
            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }

        [IsViewable] public string Name { get; set; }
    }
}