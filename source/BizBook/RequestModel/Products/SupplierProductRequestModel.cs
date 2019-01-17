using CommonLibrary.RequestModel;
using Model.Products;
using System;
using System.Linq;
using CommonLibrary.ViewModel;
using System.Linq.Expressions;
using System.Data.Entity;

namespace RequestModel.Products
{
    public class SupplierProductRequestModel : RequestModel<SupplierProduct>
    {
        public SupplierProductRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<SupplierProduct, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.ProductDetailId.Contains(Keyword) ||
                    x.SupplierId.Contains(Keyword);
            }

            if (ParentId.IdIsOk())
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.SupplierId == ParentId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<SupplierProduct, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel()
            {
                Id = x.Id,
                Text = x.ProductDetail.Name
            };
        }

        public override IQueryable<SupplierProduct> IncludeParents(IQueryable<SupplierProduct> queryable)
        {
            return queryable.Include(x => x.ProductDetail);
        }
    }
}