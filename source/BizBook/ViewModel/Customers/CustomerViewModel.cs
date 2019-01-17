using CommonLibrary.ViewModel;
using Model.Customers;

namespace ViewModel.Customers
{
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerViewModel : BaseViewModel<Customer>
    {
        public CustomerViewModel(Customer x) : base(x)
        {
            Name = x.Name;
            MembershipCardNo = x.MembershipCardNo;
            Occupation = x.Occupation;
            University = x.University;
            Company = x.Company;
            Phone = x.Phone;
            Email = x.Email;
            NationalId = x.NationalId;
            ImageUrl = x.ImageUrl;
            Remarks = x.Remarks;
            Point = x.Point;
            IsActive = x.IsActive;
            OrdersCount = x.OrdersCount;
            ProductAmount = x.ProductAmount;
            OtherAmount = x.OtherAmount;
            TotalDiscount = x.TotalDiscount;
            TotalAmount = x.TotalAmount;
            TotalPaid = x.TotalPaid;
            TotalDue = x.TotalDue;
            ShopId = x.ShopId;

            if (x.Addresses != null)
            {
                Addresses = x.Addresses.ToList().Select(y => new AddressViewModel(y)).ToList();
            }
        }

        public double OtherAmount { get; set; }

        public double TotalDiscount { get; set; }

        public double ProductAmount { get; set; }

        public double TotalPaid { get; set; }

        public double TotalDue { get; set; }

        public int OrdersCount { get; set; }

        public double TotalAmount { get; set; }

        public string ShopId { get; set; }

        public string MembershipCardNo { get; set; }

        [IsViewable] public string Name { get; set; }

        public string Occupation { get; set; }

        public string University { get; set; }

        public string Company { get; set; }

        public string District { get; set; }

        [IsViewable] public string Phone { get; set; }

        public string Email { get; set; }

        public string NationalId { get; set; }

        public string ImageUrl { get; set; }

        [IsViewable] public int Point { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public List<AddressViewModel> Addresses { get; set; }
    }
}