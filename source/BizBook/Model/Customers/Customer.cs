using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Sales;

namespace Model.Customers
{
    public class Customer : ShopChild
    {
        [Index] [Required] [MaxLength(100)] public string MembershipCardNo { get; set; }

        [Index] [MaxLength(100)] [Required] public string Name { get; set; }

        [Index] [Required] [MaxLength(100)] public string Phone { get; set; }

        [Index] [MaxLength(50)] public string Email { get; set; }
        [Index] [MaxLength(50)] public string NationalId { get; set; }
        [Index] [MaxLength(300)] public string ImageUrl { get; set; }
        [MaxLength(50)] public string Occupation { get; set; }

        [MaxLength(50)] public string University { get; set; }

        [MaxLength(50)] public string Company { get; set; }

        [Index] public int Point { get; set; }

        [MaxLength(100)] public string Remarks { get; set; }

        [Index] public int OrdersCount { get; set; }

        [Index] public double ProductAmount { get; set; }

        [Index] public double OtherAmount { get; set; }
        [Index] public double TotalDiscount { get; set; }

        [Index] public double TotalAmount { get; set; }

        [Index] public double TotalPaid { get; set; }

        [Index] public double TotalDue { get; set; }

        public int WcId { get; set; }

        public virtual ICollection<Sale> BuyingHistory { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
    }
}