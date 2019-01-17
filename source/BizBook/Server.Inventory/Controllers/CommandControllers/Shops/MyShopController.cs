using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Model;
using Model.Shops;
using RequestModel.Shops;
using Server.Inventory.Attributes;
using Server.Inventory.Filters;
using Server.Inventory.Models;
using ViewModel.Shops;

namespace Server.Inventory.Controllers.CommandControllers.Shops
{
    [BizBookAuthorization]
    [RoutePrefix("api/MyShop")]
    public class MyShopController : ApiController
    {
        public ApplicationUser AppUser;
        public BaseService<Shop, ShopRequestModel, ShopSuperAdminViewModel> Service { get; set; }

        public MyShopController()
        {
            Service = new BaseService<Shop, ShopRequestModel, ShopSuperAdminViewModel>(new BaseRepository<Shop>(BusinessDbContext.Create()));
        }

        [HttpPut]
        [Route("Edit")]
        [ActionName("Edit")]
        [EntityEditFilter]
        public IHttpActionResult MyEdit(Shop model)
        {
            Shop shop = Service.GetById(model.Id);
            shop.Modified = model.Modified;
            shop.ModifiedBy = model.ModifiedBy;
            shop.Phone = model.Phone;

            shop.StreetAddress = model.StreetAddress;
            shop.Area = model.Area;
            shop.PostCode = model.PostCode;
            shop.Thana = model.Thana;
            shop.District = model.District;
            shop.Country = model.Country;
            shop.Website = model.Website;
            shop.Email = model.Email;
            shop.Facebook = model.Facebook;
            shop.Remarks = model.Remarks;
            shop.ContactPersonName = model.ContactPersonName;
            shop.ContactPersonPhone = model.ContactPersonPhone;
            shop.About = model.About;
            shop.LogoUrl = model.LogoUrl;
            shop.HasDeliveryChain = model.HasDeliveryChain;
            shop.IsShowOrderNumber = model.IsShowOrderNumber;
            shop.IsAutoAddToCart = model.IsAutoAddToCart;
            shop.DeliveryCharge = model.DeliveryCharge;
            shop.ReceiptName = model.ReceiptName;
            shop.ChalanName = model.ChalanName;

            bool edit = Service.Edit(shop);
            return Ok(edit);
        }
    }
}
