namespace ReportModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CommonLibrary.Model;

    public class BaseReport : Entity
    {
        [Index] [Required] [MaxLength(50)] public string Value { get; set; }

        [Index]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Index] [Required] public int RowsCount { get; set; }

        [Index] [Required] [MaxLength(128)] public string ShopId { get; set; }
    }
}