namespace RequestModel.Transactions
{
    using System;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model.Transactions;

    public class AccountHeadRequestModel : RequestModel<AccountHead>
    {
        public AccountHeadRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy,
            isAscending)
        {
        }

        protected override Expression<Func<AccountHead, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Name.ToLower().Contains(Keyword) || x.AccountHeadType.ToString().ToLower().Contains(Keyword);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<AccountHead, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Name};
        }
    }
}