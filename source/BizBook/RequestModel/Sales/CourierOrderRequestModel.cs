namespace RequestModel.Sales
{
    using System;
    using System.Linq.Expressions;
    using CommonLibrary.RequestModel;
    using CommonLibrary.ViewModel;
    using Model;
    using Model.Sales;

    public class CourierOrderRequestModel : RequestModel<Sale>
    {
        public string OrderState { get; set; } = All;


        public CourierOrderRequestModel(string keyword, string orderBy = "Modified", string isAscending = "False")
            : base(keyword, orderBy, isAscending)
        {
        }

        protected override Expression<Func<Sale, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(this.Keyword))
            {
                this.ExpressionObj = x =>
                    x.OrderNumber.ToLower().Contains(this.Keyword) ||
                    x.DeliveryTrackingNo.ToLower().Contains(this.Keyword);
            }

            if (this.OrderState != All)
            {
                OrderState state = (OrderState) Enum.Parse(typeof(OrderState), this.OrderState);
                this.ExpressionObj = this.ExpressionObj.And(x => x.OrderState == state);
            }

            if (!string.IsNullOrWhiteSpace(this.DeliverymanId))
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.DeliverymanId == this.DeliverymanId);
            }

            if (!string.IsNullOrWhiteSpace(this.Thana) && this.Thana != All)
            {
                this.ExpressionObj = this.ExpressionObj.And(x => x.Address.Thana == this.Thana);
            }

            this.ExpressionObj = this.ExpressionObj.And(x => x.CourierShopId == this.ShopId);

            return this.ExpressionObj;
        }

        public string Thana { get; set; }

        public string DeliverymanId { get; set; }

        public override Expression<Func<Sale, DropdownViewModel>> Dropdown()
        {
            throw new NotImplementedException();
        }
    }
}