namespace Model.Message
{
    using System.ComponentModel.DataAnnotations;

    public class SmsHook : ShopChild
    {
        [Required] public BizSmsHook BizSmsHook { get; set; }

        [Required] [MaxLength(100)] public string Name { get; set; }

        [MaxLength(100)] public string Description { get; set; }

        [Required] public bool IsEnabled { get; set; }
    }
}