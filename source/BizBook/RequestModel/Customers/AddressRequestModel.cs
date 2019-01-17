using System;
using System.Linq.Expressions;
using Model.Customers;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;

namespace RequestModel.Customers
{
    public class AddressRequestModel : RequestModel<Address>
    {
        public AddressRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        public bool IsAddressActive { get; set; }

        protected override Expression<Func<Address, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.AddressName.ToLower().Contains(Keyword) || x.StreetAddress.ToLower().Contains(Keyword) ||
                    x.Area.ToLower().Contains(Keyword);
            }
            //if (IsAddressActive)
            //{
            //    ExpressionObj = ExpressionObj.And(x => x.IsDefault == IsAddressActive);
            //}

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(x => x.CustomerId == ParentId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Address, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel()
            {
                Id = x.Id,
                Text = x.AddressName + " (" + x.Area + ")",
                Data = new
                {
                    x.StreetAddress,
                    x.Thana,
                    x.PostCode,
                    x.District
                }
            };
        }
    }
}