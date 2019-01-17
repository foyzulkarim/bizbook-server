using System;

namespace ServiceLibrary.StateServices
{
    public enum PurchaseStates
    {
        Created, 
        Accepted,
        ReadyToShip, 
        ConfirmShip,
        Shipped,
        Received,
        Canceled
    }

    public abstract class PurchaseState
    {
        public string CommandText { get; set; }
        public string DisplayText { get; set; }
        public PurchaseStates CurrentState { get; }
        public PurchaseState NextState { get; protected set; }

        protected PurchaseState(PurchaseStates state)
        {
            CurrentState = state;
        }

        public virtual bool TryNext(PurchaseContext context)
        {
            context.State = NextState;
            return true;
        }

        public override string ToString()
        {
            return CurrentState.ToString();
        }
    }

    public class Requested : PurchaseState
    {
        public Requested(PurchaseStates state) : base(state)
        {
            NextState = new Accepted(PurchaseStates.Accepted);
            DisplayText = "Created";
            CommandText = "Create";
        }
    }
    public class Accepted : PurchaseState
    {
        public Accepted(PurchaseStates state) : base(state)
        {
            NextState = new ReadyToDepart(PurchaseStates.ReadyToShip);
            DisplayText = "Accepted";
            CommandText = "Accept Request";
        }
    }

    public class ReadyToDepart : PurchaseState
    {
        public ReadyToDepart(PurchaseStates state) : base(state)
        {
            DisplayText = "Ready to ship";
            CommandText = "Ready to ship";
            NextState = new ConfirmDepart(PurchaseStates.ConfirmShip);
        }
    }
    public class ConfirmDepart : PurchaseState
    {
        public ConfirmDepart(PurchaseStates state) : base(state)
        {
            DisplayText = "Shipment confirmed";
            CommandText = "Confirm Shipment";
            NextState = new Shipped(PurchaseStates.Shipped);
        }
    }
    public class Shipped : PurchaseState
    {
        public Shipped(PurchaseStates state) : base(state)
        {
            DisplayText = "Shipped";
            CommandText = "Ship";
            NextState = new Received(PurchaseStates.Received);
        }
    }
    public class Received : PurchaseState
    {
        public Received(PurchaseStates state) : base(state)
        {
            DisplayText = "Received";
            CommandText = "Receive products";
            NextState = null;
        }
    }

    public class Canceled : PurchaseState
    {
        public Canceled(PurchaseStates state) : base(state)
        {
            DisplayText = "Canceled";
            CommandText = "Cancel request";
            NextState = null;
        }
    }


    public class PurchaseContext
    {
        public PurchaseState State { get; set; }
        public PurchaseContext(string currentState)
        {
            PurchaseStates purchaseStates = (PurchaseStates) Enum.Parse(typeof(PurchaseStates), currentState);
            switch (purchaseStates)
            {
                case PurchaseStates.Created:
                    State = new Requested(purchaseStates);
                    break;
                case PurchaseStates.Accepted:
                    State = new Accepted(purchaseStates);
                    break;
                case PurchaseStates.ReadyToShip:
                    State = new ReadyToDepart(purchaseStates);
                    break;
                case PurchaseStates.ConfirmShip:
                    State = new ConfirmDepart(purchaseStates);
                    break;
                case PurchaseStates.Shipped:
                    State = new Shipped(purchaseStates);
                    break;
                case PurchaseStates.Received:
                    State = new Received(purchaseStates);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(purchaseStates), purchaseStates, null);
            }
        }

        public bool TryNext()
        {
            return State.TryNext(this);
        }
    }
}
