using CommonLibrary.ViewModel;
using Model;

namespace ViewModel
{
    public abstract class ShopChildViewModel<T> : BaseViewModel<T> where T : ShopChild
    {
        public string ShopId { get; set; }

        protected ShopChildViewModel(ShopChild x) : base(x)
        {
            this.ShopId = x.ShopId;
        }
    }
}
