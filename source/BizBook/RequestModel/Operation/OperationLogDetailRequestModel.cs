using CommonLibrary.RequestModel;
using Model.Operations;
using System;
using System.Linq.Expressions;
using CommonLibrary.ViewModel;
using System.Linq;
using System.Data.Entity;

namespace RequestModel.Operation
{
    public class OperationLogDetailRequestModel : RequestModel<OperationLogDetail>
    {
        public OperationLogDetailRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
             keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<OperationLogDetail, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Remarks.ToLower().Contains(Keyword) || x.ObjectIdentifier.ToLower().Contains(Keyword) || x.OperationLog.ObjectIdentifier.ToLower().Contains(Keyword) ;
            }

            if (!string.IsNullOrWhiteSpace(ParentId))
            {
                ExpressionObj = ExpressionObj.And(x => x.OperationLogId == ParentId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<OperationLogDetail, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() { Id = x.Id, Text = x.OperationType.ToString() };
        }

        public override IQueryable<OperationLogDetail> IncludeParents(IQueryable<OperationLogDetail> queryable)
        {
            return queryable.Include(x => x.OperationLog);
        }
    }
}
