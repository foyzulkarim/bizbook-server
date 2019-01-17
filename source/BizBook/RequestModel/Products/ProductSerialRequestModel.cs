using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Products;

namespace RequestModel.Products
{
    public class ProductSerialRequestModel : RequestModel<ProductSerial>
    {
        public ProductSerialRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") :
            base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<ProductSerial, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.SerialNumber.ToLower().Contains(Keyword);
            }

            if (ParentId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.ProductDetailId == ParentId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<ProductSerial, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.SerialNumber};
        }
    }
}