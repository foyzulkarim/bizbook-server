using CommonLibrary.ViewModel;
using Model.Shops;

namespace ViewModel.Shops
{
    public class BrandViewModel : BaseViewModel<Brand>
    {
        public BrandViewModel(Brand x) : base(x)
        {
            Name = x.Name;
            Address = x.Address;
            Phone = x.Phone;
            Remarks = x.Remarks;
            ContactPersonName = x.ContactPersonName;
            Country = x.Country;
            Email = x.Email;
            BrandCode = x.BrandCode;
            ShopId = x.ShopId;
            MadeInCountry = x.MadeInCountry;
        }

        public string MadeInCountry { get; set; }

        public string ShopId { get; set; }

        [IsViewable] public string BrandCode { get; set; }

        [IsViewable] public string Email { get; set; }
        [IsViewable] public string Country { get; set; }

        [IsViewable] public string Name { get; set; }
        [IsViewable] public string Address { get; set; }
        [IsViewable] public string Phone { get; set; }
        [IsViewable] public string Remarks { get; set; }
        [IsViewable] public string ContactPersonName { get; set; }
    }
}