using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.Http;
using CommonLibrary.Repository;
using CommonLibrary.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model;

using RequestModel.Shops;
using Serilog;
using Server.Identity.Filters;
using Server.Identity.Models;
using ViewModel.Shops;
using M = Model.Shops.Shop;

namespace Server.Identity.Controllers.CommandControllers.Shops
{
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/Shop")]
    public class ShopController : ApiController
    {
        public static ILogger Logger = Log.ForContext(typeof(ShopController));
        private string typeName;

        public BaseService<M, ShopRequestModel, ShopSuperAdminViewModel> Service { get; set; }

        public ShopController()
        {
            Service = new BaseService<M, ShopRequestModel, ShopSuperAdminViewModel>(new BaseRepository<M>(BusinessDbContext.Create()));
            typeName = typeof(ShopController).Name;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ActionName("Add")]
        [Route("Add")]
        [EntitySaveFilter]
        public IHttpActionResult Add(M model)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                model.RegistrationDate = DateTime.UtcNow;
                model.IsActive = true;
                model.IsDeleted = false;
                model.IsVerified = true;
                bool post = Service.Add(model);

                if (!string.IsNullOrWhiteSpace(post.ToString()))
                {
                    AddUser(model);
                    AddAccountHeads(model);
                    AddBrand(model);
                    AddSupplier(model);
                    AddProduct(model);
                    AddWarehouse(model);
                }

                scope.Complete();
                return Ok();
            }
        }



        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        [Route("Edit")]
        [ActionName("Edit")]
        [EntityEditFilter]
        public IHttpActionResult Put(M model)
        {
            M shop = Service.GetById(model.Id);
            shop.Modified = model.Modified;
            shop.ModifiedBy = model.ModifiedBy;
            if (model.ExpiryDate != DateTime.MinValue && model.ExpiryDate != DateTime.MaxValue)
            {
                shop.ExpiryDate = model.ExpiryDate;
            }
            shop.Name = model.Name;
            shop.WcUrl = model.WcUrl;
            shop.WcKey = model.WcKey;
            shop.WcSecret = model.WcSecret;
            shop.WcWebhookSource = model.WcWebhookSource;
            shop.WcVersion = model.WcVersion;

            bool edit = Service.Edit(shop);
            return Ok(edit);
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("Delete")]
        [ActionName("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                M shop = Service.GetById(id);
                shop.IsActive = false;
                shop.IsDeleted = true;
                shop.ExpiryDate = DateTime.Now;
                bool delete = Service.Edit(shop);

                Logger.Debug("Deleted entity {TypeName} with value {id}", typeName, id);
                return Ok(delete);
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception, "Exception occurred while saving {TypeName}", typeName);
                return InternalServerError(exception);
            }
        }



        private void AddUser(M model)
        {
            var userManager = Request.GetOwinContext().Get<ApplicationUserManager>();
            var shopName = Regex.Replace(model.Name.ToLower(), "[^a-zA-Z0-9]", string.Empty);
            string userName = "admin@" + shopName + "." + "bizbook365.com";
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumber = model.Phone,
                ShopId = model.Id,
                FirstName = "Admin",
                LastName = model.Name
            };

            IdentityResult result = userManager.Create(user, "Pass@" + model.Phone);
            if (result.Succeeded)
            {
                var addedToRole = userManager.AddToRole(user.Id, ApplicationRoles.ShopAdmin.ToString());
                if (addedToRole.Succeeded)
                {
                    user.RoleName = ApplicationRoles.ShopAdmin.ToString();
                    userManager.Update(user);
                }
            }
        }

        private void AddAccountHeads(M model)
        {
            var db = Request.GetOwinContext().Get<BusinessDbContext>();
            var shopId = model.Id;
            BusinessSeedData.AddAccountHeads(db, shopId);
            BusinessSeedData.AddAccountInfo(db,shopId);
        }

        private void AddBrand(M model)
        {
            var db = Request.GetOwinContext().Get<BusinessDbContext>();
            var shopId = model.Id;
            BusinessSeedData.AddBrand(shopId, db, model.Name);
        }

        private void AddSupplier(M model)
        {
            var db = Request.GetOwinContext().Get<BusinessDbContext>();
            var shopId = model.Id;
            BusinessSeedData.AddSupplier(shopId, db, model.Name);
        }

        private void AddProduct(M model)
        {
            var db = Request.GetOwinContext().Get<BusinessDbContext>();
            var shopId = model.Id;
            BusinessSeedData.AddProducts(db, shopId);
        }

        private void AddWarehouse(M model)
        {
            var db = Request.GetOwinContext().Get<BusinessDbContext>();
            var shopId = model.Id;
            BusinessSeedData.AddWarehouse(db,shopId);
        }

    }
}