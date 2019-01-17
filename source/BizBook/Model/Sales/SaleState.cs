using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Sales
{
    public class SaleState : ShopChild
    {
        [Index] [Required] [MaxLength(50)] public string State { get; set; }

        public string Remarks { get; set; }

        [Required] public string SaleId { get; set; }

        [ForeignKey("SaleId")] public virtual Sale Sale { get; set; }
    }
}