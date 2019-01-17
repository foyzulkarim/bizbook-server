using CommonLibrary.ViewModel;
using Model.Warehouses;

namespace ViewModel.Warehouses
{
    public class DamageViewModel : BaseViewModel<Damage>
    {
        public DamageViewModel(Damage x) : base(x)
        {
            ProductDetailId = x.ProductDetailId;

            if (x.ProductDetail != null)
            {
                ProductName = x.ProductDetail.Name;
            }

            WarehouseId = x.WarehouseId;

            if (x.Warehouse != null)
            {
                Name = x.Warehouse.Name;
            }

            Quantity = x.Quantity;

            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }
        public string WarehouseId { get; set; }
        public string Name { get; set; }
        public string ProductDetailId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
    }
}