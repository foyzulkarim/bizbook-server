using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Model.Products;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;


namespace RequestModel.Products
{
    public class ProductDetailRequestModel : RequestModel<ProductDetail>
    {
        public int OnHand { get; set; }

        public bool IsProductActive { get; set; }

        public string WarehouseId { get; set; }

        public bool IsRawProduct { get; set; }

        public ProductDetailRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<ProductDetail, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.ProductCategory.Name.ToLower().Contains(Keyword)
                    || x.Name.ToLower().Contains(Keyword)
                    || x.Model.ToLower().Contains(Keyword)
                    || x.BarCode.ToLower().Contains(Keyword)
                    || x.ProductCode.ToLower().Contains(Keyword);
            }

            if (IsProductActive)
            {
                ExpressionObj = ExpressionObj.And(x => x.IsActive == IsProductActive);
            }

            if (IsRawProduct)
            {
                ExpressionObj = ExpressionObj.And(x => x.IsRawProduct == IsRawProduct);
            }

            if (ParentId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.ProductCategoryId == ParentId);
            }

            if (OnHand > 0)
            {
                ExpressionObj = ExpressionObj.And(x => x.OnHand <= OnHand);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override IQueryable<ProductDetail> IncludeParents(IQueryable<ProductDetail> queryable)
        {
            return queryable.Include(x => x.ProductCategory);
        }

        public override Expression<Func<ProductDetail, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel()
            {
                Id = x.Id,
                Text = x.Name,
                Data = new {x.Id, x.Name, x.CostPrice, x.SalePrice, x.OnHand, x.BarCode}
            };
        }
    }
}