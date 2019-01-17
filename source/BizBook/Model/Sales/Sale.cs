using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Customers;
using Model.Warehouses;

namespace Model.Sales
{
    using Model.Dealers;
    using Model.Employees;
    using Transactions;

    public class Sale : ShopChild
    {
        [Index]
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string OrderNumber { get; set; }

        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string OrderReferenceNumber { get; set; }


        #region Amounts

        [Required]
        [DataType(DataType.Currency)]
        public double ProductAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double DeliveryChargeAmount { get; set; } = 0;

        [DataType(DataType.Currency)] public double TaxAmount { get; set; }

        [DataType(DataType.Currency)] public double PaymentServiceChargeAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double OtherAmount { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double TotalAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double DiscountAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double PayableTotalAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double PaidAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double DueAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double CostAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double ProfitAmount { get; set; } // payabletotal - cost

        [Required]
        [DataType(DataType.Currency)]
        public double ProfitPercent { get; set; } // (profitamount * 100) / costamount

        public double PaidByCashAmount { get; set; }

        public double PaidByOtherAmount { get; set; }

        #endregion

        #region Delivery

        [Index("IX_CourierShopId")]
        [MaxLength(128)]
        public string CourierShopId { get; set; }

        //[ForeignKey("CourierShopId")]
        //public virtual Courier CourierShop { get; set; }

        [MaxLength(50)] public string CourierName { get; set; }

        [MaxLength(50)] public string DeliveryTrackingNo { get; set; }

        [MaxLength(50)] public string DeliverymanId { get; set; }

        [MaxLength(50)] public string DeliverymanName { get; set; }

        [MaxLength(50)] public string DeliverymanPhone { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EstimatedDeliveryDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RequiredDeliveryDateByCustomer { get; set; }

        [MaxLength(50)] public string RequiredDeliveryTimeByCustomer { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OrderDate { get; set; }

        #endregion

        #region Customer

        [Index] public string CustomerId { get; set; }

        [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; }

        [Index] public string AddressId { get; set; }

        [ForeignKey("AddressId")] public virtual Address Address { get; set; }

        [Index] public string BillingAddressId { get; set; }

        [ForeignKey("BillingAddressId")] public virtual Address Billing { get; set; }

        public string CustomerArea { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerNote { get; set; }

        public string Guarantor1Id { get; set; }

        public string Guarantor2Id { get; set; }

        #endregion

        #region Dealer

        public bool IsDealerSale { get; set; }

        [Index] [MaxLength(128)] public string DealerId { get; set; }

        [ForeignKey("DealerId")] public Dealer Dealer { get; set; }

        #endregion

        [MaxLength(200)] public string Remarks { get; set; }

        public int Version { get; set; } = 0;

        [MaxLength(50)] public string ParentSaleId { get; set; }


        // type, from, order date, state,

        [Index] public SaleChannel SaleChannel { get; set; }

        [Index] public SaleFrom SaleFrom { get; set; }

        [Index] public OrderState OrderState { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }

        public virtual ICollection<SaleState> SaleStates { get; set; }

        [Index] public int? WcId { get; set; }

        [Index] public int? WcCustomerId { get; set; }

        [MaxLength(50)] public string WcOrderKey { get; set; }

        [MaxLength(50)] public string WcCartHash { get; set; }

        [MaxLength(50)] public string WcOrderStatus { get; set; }

        [NotMapped] public List<Transaction> Transactions { get; set; }

        [NotMapped] public OrderState NextOrderState { get; set; }

        [MaxLength(50)] public string EmployeeInfoName { get; set; }

        public string EmployeeInfoId { get; set; }

        [ForeignKey("EmployeeInfoId")] public virtual EmployeeInfo EmployeeInfo { get; set; }

        public string WarehouseId { get; set; }

        [ForeignKey("WarehouseId")] public virtual Warehouse Warehouse { get; set; }

        public bool IsTaggedSale { get; set; }

        public string SaleTag { get; set; }

        public string InstallmentId { get; set; }

        [ForeignKey("InstallmentId")] public virtual Installment Installment { get; set; }
    }
}