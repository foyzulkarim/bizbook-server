namespace Model.Products
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Model.Transactions;

    public class DealerProductTransaction : ShopChild
    {
        [Required] public double Amount { get; set; }

        [Required] public string TransactionId { get; set; }

        [ForeignKey("TransactionId")] public virtual Transaction Transaction { get; set; }

        [Required] public string DealerProductId { get; set; }

        [ForeignKey("DealerProductId")] public virtual DealerProduct DealerProduct { get; set; }
    }
}