namespace ReportModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AccountReport : BaseReport
    {
        #region Classifications

        [Required] [Index] [MaxLength(50)] public string AccountHeadName { get; set; }

        [Required] [Index] [MaxLength(128)] public string AccountHeadId { get; set; }

        #endregion

        #region Values

        // All 
        [Required]
        [DataType(DataType.Currency)]
        public double AmountTotalStarting { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountTotalIn { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountTotalOut { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountTotalEnding { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public int CountTotalTrx { get; set; } = 0;

        // Cash 
        [Required]
        [DataType(DataType.Currency)]
        public double AmountCashStarting { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountCashIn { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountCashOut { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountCashEnding { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public int CountCashTrx { get; set; } = 0;

        // Bank 
        [Required]
        [DataType(DataType.Currency)]
        public double AmountBankStarting { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountBankIn { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountBankOut { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountBankEnding { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public int CountBankTrx { get; set; } = 0;

        // Mobile
        [Required]
        [DataType(DataType.Currency)]
        public double AmountMobileStarting { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountMobileIn { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountMobileOut { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountMobileEnding { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public int CountMobileTrx { get; set; } = 0;

        // Other 
        [Required]
        [DataType(DataType.Currency)]
        public double AmountOtherStarting { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountOtherIn { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountOtherOut { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public double AmountOtherEnding { get; set; } = 0;

        [Required]
        [DataType(DataType.Currency)]
        public int CountOtherTrx { get; set; } = 0;

        #endregion
    }
}