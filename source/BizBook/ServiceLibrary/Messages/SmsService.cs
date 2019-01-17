using System;
using System.Linq;
using System.Text;
using Model.Message;
using RequestModel.Message;
using ViewModel.Message;
namespace ServiceLibrary.Messages
{
    using System.Net.Http;

    using CommonLibrary.Repository;
    using CommonLibrary.Service;

    using Model;
    using Model.Sales;

    using Newtonsoft.Json;

    using ViewModel.Shops;

    public class SmsService : BaseService<Sms, SmsRequestModel, SmsViewModel>
    {
        public SmsService(BaseRepository<Sms> repository)
            : base(repository)
        {
        }

        public bool HasBalance(string shopId)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            var histories = db.SmsHistorys.Where(x => x.ShopId == shopId);
            double sum = histories.Any() ? histories.Sum(x => x.Amount) : 0;
            return sum > 0;
        }

        public bool UpdateBalance(string shopId, string smsId, double amount, string text)
        {
            BusinessDbContext db = this.Repository.Db as BusinessDbContext;
            SmsHistory entity = new SmsHistory()
            {
                ShopId = shopId,
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                CreatedBy = "System",
                CreatedFrom = "BizBook",
                ModifiedBy = "System",
                Amount = amount,
                Text = text,
                SmsId = smsId
            };
            var history = db.SmsHistorys.Add(entity);
            int i = db.SaveChanges();
            return i > 0;
        }

        public bool SendSms(Sale sale, ShopViewModel shop, string apiUrl)
        {
            if (!HasBalance(sale.ShopId))
            {
                return false;
            }

            string smsText = GetTextForSale(sale, shop);
            if (string.IsNullOrWhiteSpace(smsText))
            {
                return false;
            }

            var sms = new Sms()
            {
                ShopId = sale.ShopId,
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
                CreatedBy = sale.CreatedBy,
                CreatedFrom = "BizBook",
                DeliveryStatus = "Ok",
                Ismasked = false,
                ModifiedBy = sale.ModifiedBy,
                ReasonType = SmsReasonType.Sale,
                ReasonId = sale.Id,
                ReceiverId = sale.CustomerId,
                ReceiverNumber = sale.CustomerPhone,
                SmsReceiverType = SmsReceiverType.Customer,
                Text = smsText,
            };

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var responseMessage = client.PostAsync(apiUrl, content).GetAwaiter().GetResult();
            sms.DeliveryStatus = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            bool added = base.Add(sms);
            double amount = sms.DeliveryStatus.Contains("1905") ? 0 : -0.5;
            added = this.UpdateBalance(sale.ShopId, sms.Id, amount, sms.DeliveryStatus);
            return added;
        }

        private string GetTextForSale(Sale sale, ShopViewModel shop)
        {
            string shopName = shop.Name.Length > 11 ? shop.Name.Substring(0, 11) : shop.Name;
            string text = string.Empty;

            if (sale.OrderState == OrderState.Pending)
            {
                text = $"Your order is shifted to delivery section for processing - {shopName}";
            }

            if (sale.OrderState == OrderState.OnTheWay)
            {
                text = $"Your order is on its way, you will receive a call very soon - {shopName}";
            }

            return text;
        }
    }
}
