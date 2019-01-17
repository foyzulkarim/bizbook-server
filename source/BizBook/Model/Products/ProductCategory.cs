using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    public class ProductCategory : ShopChild
    {
        [Index]
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Index] public int WcId { get; set; }

        [Index] [Required] public string ProductGroupId { get; set; }

        [ForeignKey("ProductGroupId")] public virtual ProductGroup ProductGroup { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}