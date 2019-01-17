namespace Model.Message
{
    public class SmsHistory : ShopChild
    {
        public double Amount { get; set; }

        public string SmsId { get; set; }

        public string Text { get; set; }
    }
}