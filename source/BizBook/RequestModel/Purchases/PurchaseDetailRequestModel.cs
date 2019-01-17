using System;
using System.Linq;
using System.Linq.Expressions;
using Model.Purchases;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using System.Data.Entity;

namespace RequestModel.Purchases
{
    public class PurchaseDetailRequestModel : RequestModel<PurchaseDetail>
    {
        public PurchaseDetailRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }

        public string ProductDetailId { get; set; }

        protected override Expression<Func<PurchaseDetail, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Remarks.ToLower().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                ExpressionObj = ExpressionObj.And(x => x.PurchaseId == ParentId);
            }

            if (!string.IsNullOrWhiteSpace(ProductDetailId))
            {
                ExpressionObj = ExpressionObj.And(x => x.ProductDetailId == ProductDetailId);
            }

            if (WarehouseId.IdIsOk())
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.WarehouseId == WarehouseId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public string WarehouseId { get; set; }

        public override IQueryable<PurchaseDetail> IncludeParents(IQueryable<PurchaseDetail> queryable)
        {
            return queryable.Include(x => x.ProductDetail).Include(x => x.Purchase);
        }

        public override Expression<Func<PurchaseDetail, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.ProductDetail.Name};
        }
    }
}