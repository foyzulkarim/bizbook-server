using System.ComponentModel.DataAnnotations.Schema;
using Model.Products;

namespace Model.Warehouses
{
    public class Damage : ShopChild
    {
        public string WarehouseId { get; set; }
        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }
        public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public virtual ProductDetail ProductDetail { get; set; }
        public double Quantity { get; set; }
    }
}