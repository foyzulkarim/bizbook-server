using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Transactions
{
    public class Wallet : ShopChild
    {
        [Index] [Required] [MaxLength(50)] public string AccountTitle { get; set; }


        [MaxLength(100)] public string AccountNumber { get; set; }

        [MaxLength(50)] public string BankName { get; set; }

        [Index] [Required] public WalletType WalletType { get; set; }
    }
}