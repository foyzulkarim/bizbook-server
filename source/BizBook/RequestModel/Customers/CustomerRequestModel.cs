using System;
using System.Linq.Expressions;
using Model.Customers;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;

namespace RequestModel.Customers
{
    using System.Data.Entity;
    using System.Linq;

    public class CustomerRequestModel : RequestModel<Customer>
    {
        public CustomerRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        public bool IsCustomerActive { get; set; }
        
        protected override Expression<Func<Customer, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Name.ToLower().Contains(Keyword) || x.Phone.ToLower().Contains(Keyword) ||
                    x.Email.ToLower().Contains(Keyword) || x.MembershipCardNo.ToLower().Contains(Keyword);
            }

            if (IsCustomerActive)
            {
                ExpressionObj = ExpressionObj.And(x => x.IsActive == IsCustomerActive);
            }


            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Customer, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel()
            {
                Id = x.Id,
                Text = x.Name + " (" + x.Phone + ")",
                Data = new
                {
                    x.OrdersCount,
                    OrderAmount = x.TotalAmount,
                    x.Point,
                    x.TotalPaid,
                    x.TotalDue,
                    x.TotalDiscount
                }
            };
        }

        public override IQueryable<Customer> IncludeParents(IQueryable<Customer> queryable)
        {
            return queryable.Include(x => x.Addresses).AsQueryable();
        }
    }
}