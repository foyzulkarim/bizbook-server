using System;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Message;

namespace RequestModel.Message
{
    public class SmsHistoryRequestModel : RequestModel<SmsHistory>
    {
        public SmsHistoryRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<SmsHistory, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Text.ToLower().Contains(Keyword);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);

            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());

            return ExpressionObj;
        }

        public override Expression<Func<SmsHistory, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.Text};
        }
    }
}