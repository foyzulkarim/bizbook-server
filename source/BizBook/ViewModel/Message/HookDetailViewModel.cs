using CommonLibrary.ViewModel;
using Model.Message;

namespace ViewModel.Message
{
    public class HookDetailViewModel : BaseViewModel<HookDetail>
    {
        public HookDetailViewModel(HookDetail x) : base(x)
        {
            SetProperties(x);
        }

        public void SetProperties(HookDetail x)
        {
            ShopId = x.ShopId;
            HookName = x.HookName;
            IsEnabled = x.IsEnabled;
            SmsHookId = x.SmsHookId;
        }

        public string ShopId { get; set; }
        public string HookName { get; set; }
        public bool IsEnabled { get; set; }
        public string SmsHookId { get; set; }
    }
}