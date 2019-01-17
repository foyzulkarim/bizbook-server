namespace RequestModel.Warehouses
{
    using System;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model.Warehouses;
    using System.Linq;
    using System.Data.Entity;

    public class StockTransferRequestModel : RequestModel<StockTransfer>
    {
        public StockTransferRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        public string SourceWarehouseId { get; set; }

        public string DestinationWarehoueId { get; set; }

        protected override Expression<Func<StockTransfer, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Remarks.ToLower().Contains(Keyword) || x.OrderNumber.ToString().ToLower().Contains(Keyword) ||
                    x.OrderReferenceNumber.ToString().ToLower().Contains(Keyword);
            }

            if (SourceWarehouseId.IdIsOk())
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.SourceWarehouseId == SourceWarehouseId);
            }

            if (DestinationWarehoueId.IdIsOk())
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.DestinationWarehouseId == DestinationWarehoueId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override IQueryable<StockTransfer> IncludeParents(IQueryable<StockTransfer> queryable)
        {
            return queryable
                .Include(x => x.StockTransferDetails)
                .Include(x => x.StockTransferDetails.Select(y => y.ProductDetail))
                .Include(x => x.SourceWarehouse).Include(x => x.DestinationWarehouse);
        }

        public override Expression<Func<StockTransfer, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.OrderNumber, Data = x};
        }
    }
}