using System;

namespace Model.Sales
{
    public abstract class SaleOrderState
    {
        public string Resource { get; set; }
        public string CommandText { get; set; }
        public string DisplayText { get; set; }
        public OrderState CurrentState { get; }
        public SaleOrderState NextSaleStateObject { get; protected set; }
        public SaleOrderState PreviousSaleStateObject { get; protected set; }

        protected SaleOrderState(OrderState state)
        {
            CurrentState = state;
        }

        public virtual bool TryNext(SaleContext context)
        {
            context.State = NextSaleStateObject;
            return context.State != null;
        }

        public virtual bool TryPrevious(SaleContext context)
        {
            context.State = PreviousSaleStateObject;
            return context.State != null;
        }

        public override string ToString()
        {
            return CurrentState.ToString();
        }
    }

    public class SaleContext
    {
        public SaleOrderState State { get; set; }

        public SaleContext(OrderState orderState)
        {
            //  OrderState orderState = (OrderState) Enum.Parse(typeof(OrderState), currentState);
            switch (orderState)
            {
                case OrderState.Pending:
                    State = new SalePending(orderState);
                    break;
                case OrderState.Created:
                    State = new SaleCreated(orderState);
                    break;
                case OrderState.ReadyToDeparture:
                    State = new SaleReadyToDeparture(orderState);
                    break;
                case OrderState.OnTheWay:
                    State = new SaleOnTheWay(orderState);
                    break;
                case OrderState.Delivered:
                    State = new SaleDelivered(orderState);
                    break;
                case OrderState.Completed:
                    State = new SaleCompleted(orderState);
                    break;
                case OrderState.Cancel:
                    State = new SaleCanceled(orderState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderState), orderState, null);
            }
        }

        public bool TryNext()
        {
            return State.TryNext(this);
        }

        public bool TryPrevious()
        {
            return State.TryPrevious(this);
        }
    }

    public class SaleCanceled : SaleOrderState
    {
        public SaleCanceled(OrderState state)
            : base(state)
        {
            NextSaleStateObject = null;
            DisplayText = "Cancelled";
            CommandText = "";
            Resource = "";
        }
    }

    public class SaleCompleted : SaleOrderState
    {
        public SaleCompleted(OrderState state) : base(state)
        {
            // PreviousSaleStateObject = new SaleDelivered(OrderState.Delivered);
            NextSaleStateObject = null;
            DisplayText = "Completed";
            CommandText = "";
            Resource = "";
        }
    }

    public class SaleDelivered : SaleOrderState
    {
        public SaleDelivered(OrderState state) : base(state)
        {
            // PreviousSaleStateObject = new SaleOnTheWay(OrderState.OnTheWay);
            NextSaleStateObject = new SaleCompleted(OrderState.Completed);
            DisplayText = "Delivered";
            CommandText = "Completed";
            Resource = "btn-completed";
        }
    }

    public class SaleOnTheWay : SaleOrderState
    {
        public SaleOnTheWay(OrderState state) : base(state)
        {
            // PreviousSaleStateObject = new SaleReadyToDeparture(OrderState.ReadyToDeparture);
            NextSaleStateObject = new SaleDelivered(OrderState.Delivered);
            DisplayText = "OnRoad";
            CommandText = "Delivered";
            Resource = "btn-delivered";
        }
    }

    public class SaleReadyToDeparture : SaleOrderState
    {
        public SaleReadyToDeparture(OrderState state) : base(state)
        {
            //PreviousSaleStateObject = new SaleCreated(OrderState.Created);
            NextSaleStateObject = new SaleOnTheWay(OrderState.OnTheWay);
            DisplayText = "Ready To Departure";
            CommandText = "On The Way";
            Resource = "btn-on-the-way";
        }
    }

    public class SaleCreated : SaleOrderState
    {
        public SaleCreated(OrderState state) : base(state)
        {
            //PreviousSaleStateObject = new SalePending(OrderState.Pending);
            NextSaleStateObject = new SaleReadyToDeparture(OrderState.ReadyToDeparture);
            DisplayText = "Created";
            CommandText = "Ready To Departure";
            Resource = "btn-ready-to-departure";
        }
    }

    public class SalePending : SaleOrderState
    {
        public SalePending(OrderState state) : base(state)
        {
            //PreviousSaleStateObject = null;
            NextSaleStateObject = new SaleCreated(OrderState.Created);
            DisplayText = "Pending";
            CommandText = "Create order";
            Resource = "btn-create-order";
        }
    }
}