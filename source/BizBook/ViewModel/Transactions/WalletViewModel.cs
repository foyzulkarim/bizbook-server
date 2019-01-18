using CommonLibrary.ViewModel;
using Model.Transactions;

namespace ViewModel.Transactions
{
    public class WalletViewModel : BaseViewModel<Wallet>
    {
        public WalletViewModel(Wallet x) : base(x)
        {
            AccountTitle = x.AccountTitle;
            AccountNumber = x.AccountNumber;
            BankName = x.BankName;
            WalletType = x.WalletType.ToString();
            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }
        public string AccountTitle { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string WalletType { get; set; }
    }
}