using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CommonLibrary.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model;
using Serilog;
using Server.Identity.Models;

namespace Server.Identity.Filters
{
    public class EntityEditFilterAttribute : ActionFilterAttribute
    {
        public static ILogger logger = Log.ForContext<EntityEditFilterAttribute>();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            object actionArgument = actionContext.ActionArguments["model"];
            if (actionArgument != null)
            {
                string username = actionContext.RequestContext.Principal.Identity.Name;
                var isEntity = actionArgument is Entity;
                Entity entity = actionArgument as Entity;
                if (isEntity)
                {                  
                    entity.Modified = DateTime.UtcNow;
                    entity.ModifiedBy = username;
                }

                var isShopChild = actionArgument is ShopChild;
                if (isShopChild)
                {
                    var shopChild = actionArgument as ShopChild;
                    var manager = actionContext.Request.GetOwinContext().Get<ApplicationUserManager>();
                    var id = actionContext.RequestContext.Principal.Identity.GetUserId();
                    ApplicationUser user = manager.FindById(id);
                    shopChild.ShopId = user.ShopId;
                }

                ClientRequestModel client = new ClientRequestModel(actionContext) { UserName = username };
                string clientConnectionId = client.ConnectionId;
                Thread.SetData(Thread.GetNamedDataSlot("ConnectionId"), clientConnectionId);
                Trace.TraceInformation("Trace Request: {0} {1}", client, clientConnectionId);
                string clientStr = client.ToString();
                logger.Debug("Edit request: {0} ", clientStr);
                logger.Debug("EntitySaveObject: {@Entity}", entity);
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data must not be empty");
            }

        }  
    }
}