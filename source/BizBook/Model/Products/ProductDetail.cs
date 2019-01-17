using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Purchases;
using Model.Sales;
using Model.Shops;

namespace Model.Products
{
    public class ProductDetail : ShopChild
    {
        [Index]
        [Required]
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Model { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Year { get; set; }

        [Index]
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string BarCode { get; set; }

        [Index]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string ProductCode { get; set; }

        [Required]
        public bool HasUniqueSerial { get; set; } = false;

        [Required]
        public bool HasWarrenty { get; set; } = false;

        [DataType(DataType.Currency)]
        public double SalePrice { get; set; }

        [DataType(DataType.Currency)]
        public double DealerPrice { get; set; }

        [DataType(DataType.Currency)]
        public double CostPrice { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [Required]
        public int StartingInventory { get; set; } = 0;

        [Required]
        public double Purchased { get; set; }

        [Required]
        public double Sold { get; set; }

        [Required]
        public double OnHand { get; set; }

        public int MinimumStockToNotify { get; set; }

        [Index]
        public int WcId { get; set; }

        [MaxLength(50)]
        public string Permalink { get; set; }

        [MaxLength(50)]
        public string WcType { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string ShortDescription { get; set; }

        [MaxLength(50)]
        public string Tags { get; set; }

        [Index]
        public int WcCategoryId { get; set; }

        [MaxLength(50)]
        public string RelatedIds { get; set; }

        [MaxLength(50)]
        public string WcVariationPermalink { get; set; }

        [Index]
        public int WcVariationId { get; set; }

        [MaxLength(50)]
        public string WcVariationOption { get; set; }

        [Required]
        public string ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        [Required]
        public string BrandId { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        public virtual List<ProductImage> Images { get; set; }

        [NotMapped]
        public virtual ICollection<ProductVariation> Variations { get; set; }

        [Required]
        public bool HasExpiryDate { get; set; }

        public int ExpireInDays { get; set; }

        public bool IsRawProduct { get; set; }
    }
}