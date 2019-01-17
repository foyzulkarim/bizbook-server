using System;

namespace RequestModel.Reports
{
    using System.Data.Entity;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model;
    using ReportModel;

    public class ProductReportRequestModel : BaseReportRequestModel<ProductReport>
    {
        public ProductReportType ProductReportType { get; set; }

        public bool IsProductActive { get; set; }

        public string WarehouseId { get; set; }

        public string ProductDetailId { get; set; }

        public ProductReportRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<ProductReport, bool>> GetExpression()
        {
            //if (!string.IsNullOrWhiteSpace(ProductDetailId) && ProductDetailId != new Guid().ToString())
            //{
            //    this.ExpressionObj = this.ExpressionObj.And(x => x.ProductDetailId == ProductDetailId);
            //}

            //this.ExpressionObj = this.ExpressionObj.And(x => x.ShopId == this.ShopId);
            //this.GenerateBaseEntityExpression();
            //if (StartDate != new DateTime())
            //    this.ExpressionObj = this.ExpressionObj.And(x => x.Date == StartDate);

            switch (this.ProductReportType)
            {
                case ProductReportType.ProductDetailHistory:
                    if (!(string.IsNullOrWhiteSpace(this.ProductDetailId) &&
                          this.ProductDetailId != new Guid().ToString()) && this.StartDate != new DateTime() &&
                        this.EndDate != new DateTime())
                    {
                        this.StartDate = this.StartDate.Date;
                        this.EndDate = this.EndDate.Date;

                        this.ExpressionObj = this.ExpressionObj.And(x => x.ProductDetailId == this.ProductDetailId);
                        this.ExpressionObj = this.ExpressionObj.And(x =>
                            DbFunctions.TruncateTime(x.Date) >= this.StartDate &&
                            DbFunctions.TruncateTime(x.Date) <= this.EndDate);
                    }

                    break;
                case ProductReportType.ProductDetailStockReport:
                    if (this.StartDate != new DateTime())
                    {
                        this.StartDate = this.StartDate.Date;
                        this.ExpressionObj =
                            this.ExpressionObj.And(x => DbFunctions.TruncateTime(x.Date) == this.StartDate);
                    }

                    break;
            }


            
            this.ExpressionObj = this.ExpressionObj.And(x => x.ShopId == this.ShopId);

            return this.ExpressionObj;
        }

        public override Expression<Func<ProductReport, DropdownViewModel>> Dropdown()
        {
            throw new NotImplementedException();
        }
    }
}