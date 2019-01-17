using CommonLibrary.ViewModel;
using Model.Message;

namespace ViewModel.Message
{
    public class SmsHistoryViewModel : BaseViewModel<SmsHistory>
    {
        public SmsHistoryViewModel(SmsHistory x) : base(x)
        {
            Amount = x.Amount;
            Text = x.Text;
        }

        public double Amount { get; set; }

        public string Text { get; set; }
    }
}