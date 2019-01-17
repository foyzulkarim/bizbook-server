using Model.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Products
{
    public class SupplierProductTransaction : ShopChild
    {
        [Required] public double Amount { get; set; }

        [Required] public string TransactionId { get; set; }

        [ForeignKey("TransactionId")] public virtual Transaction Transaction { get; set; }

        [Required] public string SupplierProductId { get; set; }

        [ForeignKey("SupplierProductId")] public virtual SupplierProduct SupplierProduct { get; set; }
    }
}