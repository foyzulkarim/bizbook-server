using CommonLibrary.ViewModel;
using Model.Message;

namespace ViewModel.Message
{
    using Model;

    public class SmsViewModel : BaseViewModel<Sms>
    {
        public SmsViewModel(Sms x) : base(x)
        {
            SetProperties(x);
        }

        public void SetProperties(Sms x)
        {
            ShopId = x.ShopId;
            Text = x.Text;
            SmsReceiverType = x.SmsReceiverType;
            ReceiverNumber = x.ReceiverNumber;
            ReceiverId = x.ReceiverId;
            ReasonType = x.ReasonType;
            ReasonId = x.ReasonId;
            DeliveryStatus = x.DeliveryStatus;
            Ismasked = x.Ismasked;
        }

        public string ShopId { get; set; }
        public string Text { get; set; }
        public SmsReceiverType SmsReceiverType { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverId { get; set; }
        public SmsReasonType ReasonType { get; set; }
        public string ReasonId { get; set; }
        public string DeliveryStatus { get; set; }
        public bool Ismasked { get; set; }
    }
}