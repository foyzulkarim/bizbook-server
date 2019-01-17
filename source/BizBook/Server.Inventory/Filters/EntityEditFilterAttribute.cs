using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommonLibrary.Model;
using Model;
using Serilog;
using Server.Inventory.Models;

namespace Server.Inventory.Filters
{
    public class EntityEditFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            dynamic appUser = null;
            bool tryGetValue = actionContext.Request.Properties.TryGetValue("AppUser", out appUser);
            object data = actionContext.ActionArguments["model"];
           // Log.Logger.Debug("Edit request: Data {@Data}", data);
            if (tryGetValue && data != null)
            {
                var user = appUser as ApplicationUser;
                string username = user.UserName;

                var isEntity = data is Entity;
                Entity entity = data as Entity;
                if (isEntity)
                {
                    entity.Modified = DateTime.Now;
                    entity.ModifiedBy = username;
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