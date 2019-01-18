namespace Model.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Transaction : ShopChild
    {
        [Required] [Index] public TransactionFlowType TransactionFlowType { get; set; }

        [Index] [Required] public TransactionMedium TransactionMedium { get; set; }

        [Index] [Required] public PaymentGatewayService PaymentGatewayService { get; set; }

        [Required] [Index] public TransactionFor TransactionFor { get; set; }

        [Required] [Index] public TransactionWith TransactionWith { get; set; }

        [Index] [MaxLength(50)] public string ParentId { get; set; }

        [Index] [MaxLength(50)] public string ParentName { get; set; }

        [Index] [MaxLength(50)] public string OrderNumber { get; set; }

        [Index] [MaxLength(50)] public string OrderId { get; set; }

        [Index]
        [DataType(DataType.Currency)]
        [Required]
        public double Amount { get; set; }

        [Required] [Index] [MaxLength(50)] public string TransactionMediumName { get; set; }

        [Required] [Index] [MaxLength(50)] public string PaymentGatewayServiceName { get; set; }

        [Index] [MaxLength(50)] public string TransactionNumber { get; set; }

        [MaxLength(50)] public string Remarks { get; set; }

        [MaxLength(50)] public string ContactPersonName { get; set; }

        [MaxLength(50)] public string ContactPersonPhone { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TransactionDate { get; set; }

        [Index] [MaxLength(50)] [Required] public string AccountHeadName { get; set; }

        [Required] [Index] public string AccountHeadId { get; set; }

        [ForeignKey("AccountHeadId")] public virtual AccountHead AccountHead { get; set; }

        [Index] public string WalletId { get; set; }

        [ForeignKey("WalletId")] public virtual Wallet Wallet { get; set; }
    }
}