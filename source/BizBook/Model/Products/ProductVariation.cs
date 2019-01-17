using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    public class ProductVariation
    {
        [NotMapped] public string Permalink { get; set; }
        [NotMapped] public int WcVariationId { get; set; }
        [NotMapped] public string Option { get; set; }
        [NotMapped] public double CostPrice { get; set; }
        [NotMapped] public double SalePrice { get; set; }
        [NotMapped] public int OnHand { get; set; }
        [NotMapped] public int Purchased { get; set; }
        [NotMapped] public int Sold { get; set; }
        [NotMapped] public List<ProductImage> Images { get; set; }
        [NotMapped] public string Name { get; set; }
    }
}