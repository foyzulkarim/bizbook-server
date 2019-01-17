using System;

namespace Server.Identity.Helper
{
    public class B2BSignalrEvents
    {
        public event EventHandler ChatReceived;

        public delegate void ChatReceivedEventHandler(ChatEventArgs e);

        protected virtual void OnChatReceived(ChatEventArgs e)
        {
            EventHandler handler = ChatReceived;
            handler?.Invoke(this, e);
        }

        public void Receive(string sender, string receiver, string message)
        {
            OnChatReceived(new ChatEventArgs {DateTime = DateTime.UtcNow, SenderUsername = sender, ReceiverUsername = receiver, Message = message});
        }
    }

    public class ChatEventArgs : EventArgs
    {
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class SignalrEvents
    {
        private int threshold;
        private int total;

        public SignalrEvents(int passedThreshold)
        {
            threshold = passedThreshold;
        }

        public void Add(int x)
        {
            total += x;
            if (total >= threshold)
            {
                ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                args.Threshold = threshold;
                args.TimeReached = DateTime.UtcNow;
                OnThresholdReached(args);
            }
        }

        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
    }

    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }

}