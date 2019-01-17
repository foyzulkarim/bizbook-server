using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CommonLibrary.RequestModel;
using CommonLibrary.ViewModel;
using Model.Warehouses;

namespace RequestModel.Warehouses
{
    public class DamageRequestModel : RequestModel<Damage>
    {
        public DamageRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False") : base(
            keyword, orderBy, isAscending)
        {
        }

        public string WarehouseId { get; set; }

        protected override Expression<Func<Damage, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.ProductDetail.Name.ToLower().Contains(Keyword) ||
                                     x.Warehouse.Name.ToLower().Contains(Keyword);
            }

            if (!string.IsNullOrWhiteSpace(WarehouseId))
            {
                if (WarehouseId.IdIsOk())
                {
                    ExpressionObj = ExpressionObj.And(x => x.Id == WarehouseId);
                }
            }

            ExpressionObj = ExpressionObj.And(x => x.ShopId == ShopId);
            ExpressionObj = ExpressionObj.And(GenerateBaseEntityExpression());
            return ExpressionObj;
        }

        public override Expression<Func<Damage, DropdownViewModel>> Dropdown()
        {
            return x => new DropdownViewModel() {Id = x.Id, Text = x.ProductDetail.Name};
        }

        public override IQueryable<Damage> IncludeParents(IQueryable<Damage> queryable)
        {
            return queryable.Include(x => x.Warehouse).Include(x => x.ProductDetail);
        }
    }
}