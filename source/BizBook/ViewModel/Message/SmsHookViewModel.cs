using CommonLibrary.ViewModel;
using Model.Message;

namespace ViewModel.Message
{
    public class SmsHookViewModel : BaseViewModel<SmsHook>
    {
        public SmsHookViewModel(SmsHook x) : base(x)
        {
            Name = x.Name;
            IsEnabled = x.IsEnabled;
            ShopId = x.ShopId;
        }

        public string ShopId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}