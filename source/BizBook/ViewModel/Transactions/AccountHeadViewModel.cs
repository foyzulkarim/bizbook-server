namespace ViewModel.Transactions
{
    using Model.Transactions;

    public class AccountHeadViewModel : ShopChildViewModel<AccountHead>
    {
        public AccountHeadViewModel(AccountHead x) : base(x)
        {
            Name = x.Name;
            this.AccountHeadType = x.AccountHeadType.ToString();
        }

        public string AccountHeadType { get; set; }

        public string Name { get; set; }
    }
}