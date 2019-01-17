using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;

namespace RequestModel.Shops
{
    using Model.Dealers;

    public class DealerRequestModel : RequestModel<Dealer>
    {
        public DealerRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Dealer, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.ToLower().Contains(Keyword) || x.Phone.ToLower().Contains(Keyword);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Dealer, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Name + "(" + x.Phone + ")"};
        }
    }
}