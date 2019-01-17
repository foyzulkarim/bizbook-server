using Model.Shops;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    public class SupplierProduct : ShopChild
    {
        [Required] public double Quantity { get; set; }

        [Required] public double TotalPrice { get; set; }

        [Required] public double Paid { get; set; }

        [Required] public double Due { get; set; }

        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }

        [Required] public string SupplierId { get; set; }

        [ForeignKey("SupplierId")] public Supplier Supplier { get; set; }

        public virtual ICollection<SupplierProductTransaction> Transactions { get; set; }
    }
}