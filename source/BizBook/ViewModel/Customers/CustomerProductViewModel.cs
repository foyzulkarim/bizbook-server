using ViewModel.Sales;

namespace ViewModel.Customers
{
    public class CustomerProductViewModel 
    {

        public CustomerProductViewModel(SaleDetailViewModel x, string productName, double salePrice)
        {
            
            Total = x.Total;
            Quantity = x.Quantity;
            ProductName = x.ProductDetailName;
            InvoiceNumber = x.SaleOrderNo;
            Price = x.SalePricePerUnit;
        }
        public string InvoiceNumber { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
