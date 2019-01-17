using CommonLibrary.ViewModel;

namespace ViewModel.Shops
{
    using Model.Dealers;

    public class DealerViewModel : BaseViewModel<Dealer>
    {
        public DealerViewModel(Dealer x) : base(x)
        {
            ShopId = x.ShopId;
            Area = x.Area;
            CompanyName = x.CompanyName;
            ContactPersonName = x.CompanyName;
            Country = x.Country;
            District = x.District;
            IsVerified = x.IsVerified;
            StreetAddress = x.StreetAddress;
            PostCode = x.PostCode;
            Remarks = x.Remarks;
            Name = x.Name;
            Phone = x.Phone;
            Thana = x.Thana;
        }

        public string ShopId { get; set; }
        [IsViewable] public string Name { get; set; }
        [IsViewable] public string StreetAddress { get; set; }
        [IsViewable] public string Area { get; set; }
        [IsViewable] public string Thana { get; set; }
        [IsViewable] public string PostCode { get; set; }
        [IsViewable] public string District { get; set; }
        [IsViewable] public string Country { get; set; }
        [IsViewable] public string Phone { get; set; }
        [IsViewable] public string Remarks { get; set; }
        [IsViewable] public string CompanyName { get; set; }
        [IsViewable] public string ContactPersonName { get; set; }
        [IsViewable] public bool IsVerified { get; set; }
    }
}