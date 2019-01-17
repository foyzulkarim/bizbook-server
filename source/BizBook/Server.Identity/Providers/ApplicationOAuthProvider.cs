using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Model;
using Model.Employees;
using Newtonsoft.Json;
using Serilog;
using Server.Identity.Models;

namespace Server.Identity.Providers
{
    using Model.Shops;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        //public static ILogger Logger = Log.ForContext(typeof(ApplicationOAuthProvider));
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            //Add Access-Control-Allow-Origin header as Enabling the Web Api CORS will not enable it for this provider request.
            context.OwinContext.Request.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                Log.Logger.Error("Invalid login attempt for {UserName}", context.UserName);
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }


            if (user.IsActive== false)
            {
                Log.Logger.Error("Invalid login attempt for {UserName}", context.UserName);
                context.SetError("invalid_grant", "User is Deactivated");
                return;
            }


            BusinessDbContext db = BusinessDbContext.Create();
            var shop = await db.Shops.FirstAsync(x => x.Id == user.ShopId);
            if (shop.ExpiryDate.Date < DateTime.UtcNow.Date)
            {
                Log.Logger.Error("Invalid login attempt for shop {Name}", shop.Name);
                context.SetError("shop_expired", "Shop " + shop.Name + " registration is expired.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);
            IList<string> roles = userManager.GetRoles(user.Id);
            AuthenticationProperties properties = CreateProperties(user, roles, shop, db);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            Log.Logger.Debug("Successful login for {UserName}", context.UserName);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        //public static AuthenticationProperties CreateProperties(string userName)
        //{
        //    IDictionary<string, string> data = new Dictionary<string, string>
        //    {
        //        { "userName", userName }
        //    };

        //    return new AuthenticationProperties(data);
        //}

        public static AuthenticationProperties CreateProperties(ApplicationUser user, IList<string> roles, Shop shop, BusinessDbContext businessDb)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("id", user.Id);
            data.Add("name", user.FirstName + " " + user.LastName);
            data.Add("userName", user.UserName);
            // role
            IdentityUserRole identityUserRole = user.Roles.FirstOrDefault();
            if (identityUserRole != null)
            {
                data.Add("roleId", identityUserRole.RoleId);
            }

            string roleName = roles.First();
            data.Add("role", roleName);

            data.Add("connectionId", "");

            if (string.IsNullOrWhiteSpace(user.ShopId))
            {
                user.ShopId = new Guid().ToString();
            }
            data.Add("shopId", user.ShopId);

            EmployeeInfo employeeInfo = businessDb.EmployeeInfos.FirstOrDefault(x => x.Email == user.Email && x.ShopId == user.ShopId);
            if (employeeInfo != null && !string.IsNullOrWhiteSpace(employeeInfo.WarehouseId))
            {
                data.Add("warehouseId", employeeInfo.WarehouseId);
            }

            SecurityDbContext db = SecurityDbContext.Create();
            IQueryable<ApplicationPermission> permissions = db.Permissions.Where(x => x.RoleId == identityUserRole.RoleId && x.IsAllowed);
            var resources =
                permissions.Select(x => new { name = x.Resource.Name, isAllowed = x.IsAllowed, isDisabled = x.IsDisabled })
                    .ToList();
            string allowedResources = JsonConvert.SerializeObject(resources);
            data.Add("resources", allowedResources);

            var role = db.ApplicationRoles.Find(data["roleId"]);
            if (role != null)
            {
                if (string.IsNullOrWhiteSpace(role.DefaultRoute))
                {
                    role.DefaultRoute = "root.home";
                }
                data.Add("defaultRoute", role.DefaultRoute);
            }

            if (shop != null)
            {
                if (!string.IsNullOrWhiteSpace(shop.ChalanName))
                {
                    data.Add("ChalanName", shop.ChalanName);
                }

                if (!string.IsNullOrWhiteSpace(shop.ReceiptName))
                {
                    data.Add("ReceiptName", shop.ReceiptName);
                }

                data.Add("ShowOrderNumberAfterSave", shop.IsShowOrderNumber.ToString());
                data.Add("AddToCartIfResultIsOne", shop.IsAutoAddToCart.ToString());
                data.Add("DeliveryChargeAmount", shop.DeliveryCharge.ToString(CultureInfo.InvariantCulture));

            }

            return new AuthenticationProperties(data);
        }

    }
}