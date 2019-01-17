using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RequestModel
{
    public class PartnerRequestModel : RequestModelBase<Partner>
    {
        public PartnerRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Partner, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.PartnerShopId.ToLower().Contains(Keyword) || x.ShopId.ToLower().Contains(Keyword);
            }
            if (!string.IsNullOrWhiteSpace(Id))
            {
                ExpressionObj = ExpressionObj.And(x => x.Id == Id);
            }
            return ExpressionObj;
        }

    }
}
