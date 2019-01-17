namespace RequestModel.Transactions
{
    using System;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model.Transactions;

    public class TransactionRequestModel : RequestModel<Transaction>
    {
        public string AccountHeadId { get; set; }

        public string AccountInfoId { get; set; }
        public string TransactionMediumName { get; set; }
    

        public TransactionRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy,
            isAscending)
        {
        }

        protected override Expression<Func<Transaction, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj =
                    x =>
                        x.OrderNumber.ToLower().Contains(Keyword) || x.TransactionNumber.ToLower().Contains(Keyword) ||
                        x.AccountHeadName.ToString().ToLower().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(AccountHeadId))
            {
                ExpressionObj = ExpressionObj.And(x => x.AccountHeadId == AccountHeadId);
            }

            if (!string.IsNullOrWhiteSpace(AccountInfoId))
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.AccountInfoId == AccountInfoId);
            }

            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                ExpressionObj = ExpressionObj.And(x => x.ParentId == ParentId);
            }

            if (!string.IsNullOrWhiteSpace(TransactionMediumName))
            {
                ExpressionObj = ExpressionObj.And(x => x.TransactionMediumName == TransactionMediumName);
            }
             
            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Transaction, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.OrderNumber};
        }
    }
}