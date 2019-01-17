using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    public class ProductGroup : ShopChild
    {
        [Index]
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}