using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Products;
using Model.Warehouses;

namespace Model.Purchases
{
    public class PurchaseDetail : ShopChild
    {
        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }

        [Required] public string PurchaseId { get; set; }

        [ForeignKey("PurchaseId")] public virtual Purchase Purchase { get; set; }

        [Required] public double Quantity { get; set; }

        [Required] public double CostPricePerUnit { get; set; }

        [Required] public double CostTotal { get; set; }

        [Required] public double Paid { get; set; }

        [Required] public double Payable { get; set; }

        [MaxLength(50)] public string Remarks { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }
    }
}