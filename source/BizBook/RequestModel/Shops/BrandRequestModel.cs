using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Shops;

namespace RequestModel.Shops
{
    public class BrandRequestModel : RequestModel<Brand>
    {
        public BrandRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }


        protected override Expression<Func<Brand, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Name.ToLower().Contains(Keyword) || x.Address.ToLower().Contains(Keyword) ||
                    x.Phone.ToLower().Contains(Keyword) || x.ContactPersonName.ToLower().Contains(Keyword);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Brand, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Name};
        }
    }
}