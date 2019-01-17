using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Purchases;

namespace Model.Shops
{
    public class Supplier : ShopChild
    {
        [Index]
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)] public string StreetAddress { get; set; }

        [MaxLength(50)] public string Area { get; set; }

        [MaxLength(50)] public string Thana { get; set; }

        [MaxLength(50)] public string PostCode { get; set; }

        [MaxLength(50)] public string District { get; set; }

        [MaxLength(50)] public string Country { get; set; }

        [Index] [MaxLength(50)] public string Phone { get; set; }

        [MaxLength(50)] public string Remarks { get; set; }

        [MaxLength(50)] public string CompanyName { get; set; }

        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string ContactPersonName { get; set; }

        public bool IsVerified { get; set; }

        #region Amounts

        [Index] public int OrdersCount { get; set; }

        [Index] public double ProductAmount { get; set; }

        [Index] public double OtherAmount { get; set; }
        [Index] public double TotalDiscount { get; set; }

        [Index] public double TotalAmount { get; set; }

        [Index] public double TotalPaid { get; set; }

        [Index] public double TotalDue { get; set; }

        #endregion

        [Index] [MaxLength(128)] public string SupplierShopId { get; set; }

        [ForeignKey("SupplierShopId")] public virtual Shop SupplierShop { get; set; }

        // associated list
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}