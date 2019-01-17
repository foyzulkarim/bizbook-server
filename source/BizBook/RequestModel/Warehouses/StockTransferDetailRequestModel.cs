using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Warehouses;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RequestModel.Warehouses
{
    public class StockTransferDetailRequestModel : RequestModel<StockTransferDetail>
    {
        public StockTransferDetailRequestModel(string keyword, string orderBy = "Modified",
            string isAscending = "False") : base(keyword, orderBy, isAscending)
        {
        }

        public string ProductDetailId { get; set; }
        public string SourceWarehouseId { get; set; }
        public string DestinationWarehouseId { get; set; }

        protected override Expression<Func<StockTransferDetail, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Remarks.ToLower().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                ExpressionObj = ExpressionObj.And(x => x.StockTransferId == ParentId);
            }

            if (ProductDetailId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.ProductDetailId == ProductDetailId);
            }

            if (SourceWarehouseId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.SourceWarehouseId == SourceWarehouseId);
            }

            if (DestinationWarehouseId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.DestinationWarehouseId == DestinationWarehouseId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<StockTransferDetail, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel {Id = x.Id, Text = x.ProductDetail.Name};
        }

        public override IQueryable<StockTransferDetail> IncludeParents(IQueryable<StockTransferDetail> queryable)
        {
            return queryable.Include(x => x.ProductDetail).Include(x => x.StockTransfer).Include(x => x.SourceWarehouse)
                .Include(x => x.DestinationWarehouse);
        }
    }
}