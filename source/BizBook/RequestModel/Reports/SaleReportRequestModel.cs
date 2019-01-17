namespace RequestModel.Reports
{
    using System;
    using System.Data.Entity;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model;
    using ReportModel;
    using ReportModel.Parameters;

    public class SaleReportRequestModel : BaseReportRequestModel<SaleReport>
    {
        public SaleType SaleType { get; set; }
        public SaleReportType SaleReportType { get; set; }

        public SaleChannel SaleChannel { get; set; }

        public SaleFrom SaleFrom { get; set; }

        public SaleReportRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<SaleReport, bool>> GetExpression()
        {
            StartDate = StartDate.Date;
            EndDate = EndDate.Date;
            this.ExpressionObj = this.ExpressionObj.And(x => x.SaleType == SaleType);
            this.ExpressionObj = this.ExpressionObj.And(x => x.ShopId == this.ShopId);
            this.ExpressionObj = this.ExpressionObj.And(
                x => DbFunctions.TruncateTime(x.Date) >= StartDate && DbFunctions.TruncateTime(x.Date) <= EndDate);
            return this.ExpressionObj;
        }

        public override Expression<Func<SaleReport, DropdownViewModel>> Dropdown()
        {
            throw new NotImplementedException();
        }
    }
}