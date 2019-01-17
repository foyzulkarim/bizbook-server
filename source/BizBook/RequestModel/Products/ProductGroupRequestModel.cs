using System;
using System.Linq.Expressions;
using Model.Products;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;

namespace RequestModel.Products
{
    public class ProductGroupRequestModel : RequestModel<ProductGroup>
    {
        public ProductGroupRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }
        
        public bool IsProductGroupActive { get; set; }

        protected override Expression<Func<ProductGroup, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.ToLower().Contains(Keyword);
            }
            if (IsProductGroupActive)
            {
                ExpressionObj = ExpressionObj.And(x => x.IsActive == IsProductGroupActive);
            }
            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<ProductGroup, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Name};
        }
    }
}