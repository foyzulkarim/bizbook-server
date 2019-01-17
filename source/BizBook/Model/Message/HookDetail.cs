using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Message
{
    public class HookDetail : ShopChild
    {
        [Required] [Index] public string SmsHookId { get; set; }

        [ForeignKey("SmsHookId")] public virtual SmsHook SmsHook { get; set; }

        [Index] [Required] [MaxLength(200)] public string HookName { get; set; }

        [Required] public bool IsEnabled { get; set; }
    }
}