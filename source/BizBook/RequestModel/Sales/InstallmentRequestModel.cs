using System;
using System.Linq;

namespace RequestModel.Sales
{
    using System.Data.Entity;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model.Sales;

    public class InstallmentRequestModel : RequestModel<Installment>
    {
        public InstallmentRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Installment, bool>> GetExpression()
        {
            //if (!string.IsNullOrWhiteSpace(Keyword))
            //{
            //    ExpressionObj = x => x.Sale.OrderNumber.ToLower().Contains(Keyword) || x.Sale.OrderReferenceNumber.ToLower().Contains(Keyword) || x.Sale.CustomerPhone.ToLower().Contains(Keyword) || x.Sale.CustomerName.ToLower().Contains(Keyword);
            //}

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override IQueryable<Installment> IncludeParents(IQueryable<Installment> queryable)
        {
            return queryable.Include(x => x.InstallmentDetails);
        }

        public override Expression<Func<Installment, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Data = x, Id = x.Id, Text = x.ToString()};
        }
    }
}