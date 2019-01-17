using System.Data.Entity;

namespace RequestModel.Reports
{
    using System;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model;
    using ReportModel;

    public class AccountReportRequestModel : BaseReportRequestModel<AccountReport>
    {
        public AccountReportType AccountReportType { get; set; }

        public string AccountHeadId { get; set; }

        public AccountReportRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy,
            isAscending)
        {
        }

        protected override Expression<Func<AccountReport, bool>> GetExpression()
        {
            //if (!string.IsNullOrWhiteSpace(this.Keyword))
            //{
            //    this.ExpressionObj =
            //        x =>
            //            x.AccountHeadName.ToString().ToLower().Contains(this.Keyword);
            //}

            //if (!string.IsNullOrWhiteSpace(this.ParentId))
            //{
            //    this.ExpressionObj = this.ExpressionObj.And(x => x.AccountHeadId == this.ParentId);
            //}

            //this.ExpressionObj = this.ExpressionObj.And(x => x.ShopId == this.ShopId);
            //this.GenerateBaseEntityExpression();
            //if (StartDate != new DateTime())
            //    this.ExpressionObj = this.ExpressionObj.And(x => x.Date >= StartDate && x.Date <= EndDate);
            switch (this.AccountReportType)
            {
                case AccountReportType.TransactionDeatil:
                    if (this.StartDate != new DateTime())
                    {
                        this.StartDate = this.StartDate.Date;
                        this.ExpressionObj =
                            this.ExpressionObj.And(x => DbFunctions.TruncateTime(x.Date) == this.StartDate);
                    }

                    break;
                case AccountReportType.TransactionHistory:
                    this.StartDate = this.StartDate.Date;
                    this.EndDate = this.EndDate.Date;

                    this.ExpressionObj = this.ExpressionObj.And(x =>
                        DbFunctions.TruncateTime(x.Date) >= this.StartDate &&
                        DbFunctions.TruncateTime(x.Date) <= this.EndDate);

                    if (!string.IsNullOrWhiteSpace(this.AccountHeadId) && this.AccountHeadId != new Guid().ToString())
                    {
                        this.ExpressionObj = this.ExpressionObj.And(x => x.AccountHeadId == this.AccountHeadId);
                    }

                    break;
            }

            this.ExpressionObj = this.ExpressionObj.And(x => x.ShopId == this.ShopId);
            return this.ExpressionObj;
        }

        public override Expression<Func<AccountReport, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id};
        }
    }
}