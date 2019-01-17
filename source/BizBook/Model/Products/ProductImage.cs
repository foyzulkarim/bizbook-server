namespace Model.Products
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductImage : ShopChild
    {
        public int WcId { get; set; }

        [Required] [MaxLength(500)] public string Src { get; set; }
        [MaxLength(50)] public string Name { get; set; }
        [MaxLength(50)] public string Alt { get; set; }
        public int? Position { get; set; }


        [Required] public string ProductDetailId { get; set; }
        [ForeignKey("ProductDetailId")] public ProductDetail ProductDetail { get; set; }
    }
}