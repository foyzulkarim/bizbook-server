using CommonLibrary.ViewModel;
using Model.Shops;

namespace ViewModel.Shops
{
    public class CourierViewModel : BaseViewModel<Courier>
    {
        public CourierViewModel(Courier x) : base(x)
        {
            ShopId = x.ShopId;
            ContactPersonName = x.ContactPersonName;
            CourierShopId = x.CourierShopId;
            if (x.CourierShop != null)
            {
                CourierShopName = x.CourierShop.Name;
                CourierShopPhone = x.CourierShop.Phone;
            }
        }

        public string CourierShopName { get; set; }

        public string CourierShopPhone { get; set; }

        public string ShopId { get; set; }

        [IsViewable] public string ContactPersonName { get; set; }

        [IsViewable] public string CourierShopId { get; set; }
    }
}