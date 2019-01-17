using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Shops;

namespace RequestModel.Shops
{
    public class ShopRequestModel : RequestModel<Shop>
    {
        public bool HasDeliveryChain { get; set; }

        public ShopRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Shop, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Name.ToLower().Contains(Keyword) || x.Phone.ToLower().Contains(Keyword) ||
                    x.ContactPersonName.ToLower().Contains(Keyword);
            }

            if (HasDeliveryChain)
            {
                ExpressionObj = this.ExpressionObj.And(x => x.HasDeliveryChain);
            }

            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Shop, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Name};
        }
    }
}