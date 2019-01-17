using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Customers;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RequestModel.Customers
{
    public class CustomerFeedbackRequestModel : RequestModel<CustomerFeedback>
    {
        public CustomerFeedbackRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        public string CustomerId { get; set; }

        protected override Expression<Func<CustomerFeedback, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.OrderNumber.ToLower().Contains(Keyword) || x.Feedback.ToLower().Contains(Keyword) ||
                    x.ManagerComment.ToLower().Contains(Keyword);
            }

            if (IsIncludeParents)
            {
                ExpressionObj = ExpressionObj.Or(x => x.Customer.Name.ToLower().Contains(Keyword) || x.Customer.Phone.ToLower().Contains(Keyword));
            }

            if (CustomerId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.CustomerId == CustomerId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override IQueryable<CustomerFeedback> IncludeParents(IQueryable<CustomerFeedback> queryable)
        {
            return queryable.Include(x => x.Customer);
        }

        public override Expression<Func<CustomerFeedback, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel()
            {
                Id = x.Id
            };
        }
    }
}
