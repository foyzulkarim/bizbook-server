using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Customers
{
    public class CustomerFeedback : ShopChild
    {
        [Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public string OrderNumber { get; set; }

        [MaxLength(500)]
        public string Feedback { get; set; }

        public FeedbackType FeedbackType { get; set; }

        [MaxLength(500)]
        public string ManagerComment { get; set; }
    }
}
