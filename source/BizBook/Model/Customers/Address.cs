namespace Model.Customers
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address : ShopChild
    {
        [MaxLength(50)] public string AddressName { get; set; }

        public bool IsDefault { get; set; }

        [MaxLength(100)] public string ContactName { get; set; }

        [MaxLength(100)] public string ContactPhone { get; set; }

        [MaxLength(500)] public string StreetAddress { get; set; }

        [MaxLength(50)] public string Area { get; set; }

        [MaxLength(50)] public string Thana { get; set; }

        [MaxLength(20)] public string PostCode { get; set; }

        [MaxLength(50)] public string District { get; set; }

        [MaxLength(50)] public string Country { get; set; }

        [MaxLength(150)] public string SpecialNote { get; set; }

        [Required] public string CustomerId { get; set; }

        [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; }
    }
}