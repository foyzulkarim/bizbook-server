namespace Model.Shops
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Courier : ShopChild
    {
        [MaxLength(50)] public string ContactPersonName { get; set; }

        [Required] public string CourierShopId { get; set; }

        [ForeignKey("CourierShopId")] public virtual Shop CourierShop { get; set; }
    }
}