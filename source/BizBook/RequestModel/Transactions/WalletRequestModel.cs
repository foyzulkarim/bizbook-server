using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Transactions;

namespace RequestModel.Transactions
{
    public class WalletRequestModel : RequestModel<Wallet>
    {
        public WalletRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Wallet, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.AccountTitle.ToLower().Contains(Keyword) ||
                    x.WalletType.ToString().ToLower().Contains(Keyword);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Wallet, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.AccountTitle};
        }
    }
}