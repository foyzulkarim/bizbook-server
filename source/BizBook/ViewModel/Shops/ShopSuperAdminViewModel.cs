using System;
using CommonLibrary.ViewModel;
using Model.Shops;

namespace ViewModel.Shops
{
    public class ShopSuperAdminViewModel : ShopViewModel
    {
        public ShopSuperAdminViewModel(Shop x) : base(x)
        {
            ExpiryDate = x.ExpiryDate.Date;
            WcWebhookSource = x.WcWebhookSource;
            WcUrl = x.WcUrl;
            WcKey = x.WcKey;
            WcSecret = x.WcSecret;
            Name = x.Name;
        }

        public string WcWebhookSource { get; set; }

        public string WcSecret { get; set; }

        public string WcKey { get; set; }

        public string WcUrl { get; set; }

        public DateTime ExpiryDate { get; set; }
    }

    public class ShopViewModel : BaseViewModel<Shop>
    {
        public ShopViewModel(Shop x) : base(x)
        {
            Name = x.Name;

            StreetAddress = x.StreetAddress;
            Area = x.Area;
            Thana = x.Thana;
            PostCode = x.PostCode;
            District = x.District;
            Country = x.Country;
            Phone = x.Phone;
            Website = x.Website;
            Facebook = x.Facebook;
            Email = x.Email;
            About = x.About;
            ContactPersonName = x.ContactPersonName;
            ContactPersonPhone = x.ContactPersonPhone;
            ContactPersonDesignation = x.ContactPersonDesignation;
            LogoUrl = x.LogoUrl;
            Address = x.StreetAddress + "," + x.Area + "," + x.Thana + "," + x.PostCode + "," + x.District;
            Remarks = x.Remarks;
            HasDeliveryChain = x.HasDeliveryChain;
            ExpiryDate = x.ExpiryDate.Date;
            IsShowOrderNumber = x.IsShowOrderNumber;
            IsAutoAddToCart = x.IsAutoAddToCart;
            DeliveryCharge = x.DeliveryCharge;
            ReceiptName = x.ReceiptName;
            ChalanName = x.ChalanName;
        }

        public string ContactPersonDesignation { get; set; }

        public string ContactPersonPhone { get; set; }

        public string StreetAddress { get; set; }

        public string PostCode { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Facebook { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }

        public string Thana { get; set; }

        public string District { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Remarks { get; set; }

        public string ContactPersonName { get; set; }

        public string About { get; set; }

        public string LogoUrl { get; set; }

        public bool HasDeliveryChain { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsShowOrderNumber { get; set; }

        public bool IsAutoAddToCart { get; set; }

        public float DeliveryCharge { get; set; }

        public string ReceiptName { get; set; }

        public string ChalanName { get; set; }
    }
}