using CommonLibrary.ViewModel;
using Model.Customers;

namespace ViewModel.Customers
{
    public class AddressViewModel : BaseViewModel<Address>
    {
        public AddressViewModel(Address x) : base(x)
        {
            AddressName = x.AddressName;
            ContactName = x.ContactName;
            ContactPhone = x.ContactPhone;
            StreetAddress = x.StreetAddress;
            Area = x.Area;
            Thana = x.Thana;
            PostCode = x.PostCode;
            District = x.District;
            Country = x.Country;
            SpecialNote = x.SpecialNote;
            CustomerId = x.CustomerId;
            ShopId = x.ShopId;
            IsDefault = x.IsDefault;
            IsActivate = x.IsActive;
        }

        public string ShopId { get; set; }

        public string CustomerId { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string StreetAddress { get; set; }

        public string Area { get; set; }

        public string Thana { get; set; }

        public string PostCode { get; set; }

        public string District { get; set; }

        public string Country { get; set; }

        public string SpecialNote { get; set; }

        public string AddressName { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActivate { get; set; }
    }
}