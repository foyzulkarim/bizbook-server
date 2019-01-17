using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Operations;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RequestModel.Operation
{
    public class OperationLogRequestModel : RequestModel<OperationLog>
    {
        public OperationLogRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        public string ObjectId { get; set; }

        protected override Expression<Func<OperationLog, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x =>
                    x.Remarks.ToLower().Contains(Keyword) || x.ObjectIdentifier.ToLower().Contains(Keyword) ||
                    x.ModelName.ToString().ToLower().Contains(Keyword);
            }
            if (ObjectId.IdIsOk())
            {
                ExpressionObj = ExpressionObj.And(x => x.ObjectId == ObjectId);
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }
        public override IQueryable<OperationLog> IncludeParents(IQueryable<OperationLog> queryable)
        {
            return queryable
                .Include(x => x.OperationLogDetails);
               
        }
        public override Expression<Func<OperationLog, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() { Id = x.Id, Text = x.OperationType.ToString() };
        }
    }
}
