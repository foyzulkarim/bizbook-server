using CommonLibrary.ViewModel;
using Model.Shops;

namespace ViewModel.Shops
{
    public class SupplierViewModel : BaseViewModel<Supplier>
    {
        public SupplierViewModel(Supplier x) : base(x)
        {
            Name = x.Name;
            Address = x.StreetAddress;
            Phone = x.Phone;
            Remarks = x.Remarks;
            ContactPersonName = x.ContactPersonName;
            ShopId = x.ShopId;
            Country = x.Country;
        }

        public string ShopId { get; set; }

        [IsViewable] public string Name { get; set; }
        [IsViewable] public string Address { get; set; }
        [IsViewable] public string Phone { get; set; }
        [IsViewable] public string Remarks { get; set; }
        [IsViewable] public string ContactPersonName { get; set; }
        [IsViewable] public string Country { get; set; }
    }
}