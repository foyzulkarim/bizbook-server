using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Purchases;

namespace Model.Products
{
    public class ProductSerial : ShopChild
    {
        [Index] [Required] [MaxLength(50)] public string SerialNumber { get; set; }

        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public virtual ProductDetail ProductDetail { get; set; }

        public string PurchaseDetailId { get; set; }

        [ForeignKey("PurchaseDetailId")] public virtual PurchaseDetail PurchaseDetail { get; set; }
    }
}