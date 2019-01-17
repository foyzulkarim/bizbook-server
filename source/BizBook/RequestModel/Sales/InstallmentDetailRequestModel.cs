using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Sales;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RequestModel.Sales
{
    public class InstallmentDetailRequestModel : RequestModel<InstallmentDetail>
    {
        public DateTime ScheduledDate { get; set; }

        public InstallmentDetailRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<InstallmentDetail, bool>> GetExpression()
        {
            if (ScheduledDate != new DateTime())
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.ScheduledDate.Date == ScheduledDate.Date);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override IQueryable<InstallmentDetail> IncludeParents(IQueryable<InstallmentDetail> queryable)
        {
            return queryable.Include(x => x.Installment);
        }

        public override Expression<Func<InstallmentDetail, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Data = x, Id = x.Id, Text = x.ToString()};
        }
    }
}