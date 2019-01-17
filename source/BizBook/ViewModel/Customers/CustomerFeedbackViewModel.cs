using CommonLibrary.ViewModel;
using Model;
using Model.Customers;

namespace ViewModel.Customers
{
    public class CustomerFeedbackViewModel : BaseViewModel<CustomerFeedback>
    {
        public CustomerFeedbackViewModel(CustomerFeedback x) : base(x)
        {
            CustomerId = x.CustomerId;
            OrderNumber = x.OrderNumber;
            Feedback = x.Feedback;
            FeedbackType = x.FeedbackType;
            ManagerComment = x.ManagerComment;

            if (x.Customer != null)
            {
                CustomerName = x.Customer.Name;
                CustomerPhone = x.Customer.Phone;
            }
        }

         public string CustomerId { get; set; }

        public string OrderNumber { get; set; }

         public string Feedback { get; set; }

        public FeedbackType FeedbackType { get; set; }

        public string ManagerComment { get; set; }

        [IsViewable] public string CustomerName { get; set; }

        [IsViewable] public string CustomerPhone { get; set; }
    }
}
