using System;
using System.Runtime.InteropServices;
using Model;

namespace ServiceLibrary.StateServices
{
    public abstract class SaleState
    {
        public string CommandText { get; set; }
        public string DisplayText { get; set; }
        public OrderState CurrentState { get; }
        public SaleState NextSaleStateObject { get; protected set; }

        protected SaleState(OrderState state)
        {
            CurrentState = state;
        }

        public virtual bool TryNext(SaleContext context)
        {
            context.State = NextSaleStateObject;
            return context.State != null;
        }

        public override string ToString()
        {
            return CurrentState.ToString();
        }        
    }
    
    public class SaleContext
    {
        public SaleState State { get; set; }
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderState), orderState, null);
            }
        }

        public bool TryNext()
        {
            return State.TryNext(this);
        }
    }

    public class SaleCompleted : SaleState
    {
        public SaleCompleted(OrderState state) : base(state)
        {
            NextSaleStateObject = null;
            DisplayText = "Completed";
            CommandText = "";
        }
    }

    public class SaleDelivered : SaleState
    {
        public SaleDelivered(OrderState state) : base(state)
        {
            NextSaleStateObject=new SaleCompleted(OrderState.Completed);
            DisplayText = "Delivered";
            CommandText = "Completed";
        }
    }

    public class SaleOnTheWay : SaleState
    {
        public SaleOnTheWay(OrderState state) : base(state)
        {
            NextSaleStateObject = new SaleDelivered(OrderState.Delivered);
            DisplayText = "On Road";
            CommandText = "Delivered";
        }
    }

    public class SaleReadyToDeparture : SaleState
    {
        public SaleReadyToDeparture(OrderState state) : base(state)
        {
            NextSaleStateObject = new SaleOnTheWay(OrderState.OnTheWay);
            DisplayText = "Assigned";
            CommandText = "On The Way";
        }
    }

    public class SaleCreated : SaleState
    {
        public SaleCreated(OrderState state) : base(state)
        {
            NextSaleStateObject = new SaleReadyToDeparture(OrderState.ReadyToDeparture);
            DisplayText = "Created";
            CommandText = "Assign Deliveryman";
        }
    }

    public class SalePending : SaleState
    {
        public SalePending(OrderState state): base(state)
        {
            NextSaleStateObject = new SaleCreated(OrderState.Created);
            DisplayText = "Pending";
            CommandText = "Create order";
        }
    }
}
