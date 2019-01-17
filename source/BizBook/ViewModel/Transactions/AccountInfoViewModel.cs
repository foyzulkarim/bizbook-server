using CommonLibrary.ViewModel;
using Model.Transactions;

namespace ViewModel.Transactions
{
    public class AccountInfoViewModel : BaseViewModel<AccountInfo>
    {
        public AccountInfoViewModel(AccountInfo x) : base(x)
        {
            AccountTitle = x.AccountTitle;
            AccountNumber = x.AccountNumber;
            BankName = x.BankName;
            AccountInfoType = x.AccountInfoType.ToString();
            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }
        public string AccountTitle { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountInfoType { get; set; }
    }
}