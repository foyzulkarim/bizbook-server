using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommonLibrary.Model;
using Model;
using Server.Inventory.Models;

namespace Server.Inventory.Filters
{
    using System.Configuration;

    using Serilog;

    public class EntitySaveFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            object data = actionContext.ActionArguments["model"];
            //Log.Logger.Debug("Save request: Data {@Data}", data);
            dynamic appUser = null;
            bool tryGetValue = actionContext.Request.Properties.TryGetValue("AppUser", out appUser);

            if (tryGetValue && data != null)
            {
                var user = appUser as ApplicationUser;
                string username = user.UserName;
                string isDataMigrationStr = ConfigurationManager.AppSettings["IsDataMigration"];
                bool isDataMigration = Convert.ToBoolean(isDataMigrationStr);
                if (!isDataMigration)
                {
                    var isEntity = data is Entity;
                    Entity entity = data as Entity;
                    if (isEntity)
                    {
                        entity.Id = Guid.NewGuid().ToString();
                        entity.Created = DateTime.Now;
                        entity.Modified = DateTime.Now;
                        entity.CreatedBy = username;
                        entity.ModifiedBy = username;
                        entity.IsActive = true;
                    }
                }


                var isShopChild = data is ShopChild;
                if (isShopChild)
                {
                    var shopChild = data as ShopChild;
                    shopChild.ShopId = user.ShopId;
                }

                ClientRequestModel client =
                    new ClientRequestModel(actionContext) { UserName = username, ShopId = user.ShopId };
                string clientConnectionId = client.ConnectionId;
                Thread.SetData(Thread.GetNamedDataSlot("ConnectionId"), clientConnectionId);
                appUser.ConnectionId = clientConnectionId;
                // Log.Logger.Debug("AppUser : {@AppUser}", appUser);
                Log.Logger.Debug("Client details: {@Client} ", client);
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data must not be empty");
            }
        }
    }
}