using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Message
{
    public class Sms : ShopChild
    {
        [Required] [MaxLength(200)] public string Text { get; set; }

        [Required] public SmsReceiverType SmsReceiverType { get; set; } = SmsReceiverType.Unknown;

        [Index] [MaxLength(20)] public string ReceiverNumber { get; set; }

        [Index] [Required] [MaxLength(128)] public string ReceiverId { get; set; }

        [Index] [Required] public SmsReasonType ReasonType { get; set; } = SmsReasonType.Unknown;

        [Index] [Required] [MaxLength(128)] public string ReasonId { get; set; }

        [Required] [MaxLength(100)] public string DeliveryStatus { get; set; }

        [Required] public bool Ismasked { get; set; }

        [NotMapped] public string SenderApp { get; set; } = "BizBook";
    }
}