namespace Model.Transactions
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AccountHead : ShopChild
    {
        [Index] [Required] [MaxLength(50)] public string Name { get; set; }

        [Index] [Required] public AccountHeadType AccountHeadType { get; set; }
    }
}