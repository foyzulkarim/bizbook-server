using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    using Model.Dealers;

    public class DealerProduct : ShopChild
    {
        [Required] public double Quantity { get; set; }

        [Required] public double TotalPrice { get; set; }

        [Required] public double Paid { get; set; }

        [Required] public double Due { get; set; }

        [Required] public string ProductDetailId { get; set; }

        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }

        [Required] public string DealerId { get; set; }

        [ForeignKey("DealerId")] public Dealer Dealer { get; set; }

        public virtual ICollection<DealerProductTransaction> Transactions { get; set; }
    }
}