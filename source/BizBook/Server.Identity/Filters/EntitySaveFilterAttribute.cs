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
    public class EntitySaveFilterAttribute : ActionFilterAttribute
    {
        public static ILogger logger = Log.ForContext<EntitySaveFilterAttribute>();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            object data = actionContext.ActionArguments["model"];

            logger.Information("Save request: User {Name}, Data {@Data}",
                actionContext.RequestContext.Principal.Identity.Name, data);
            
            if (data != null)
            {
                string username = actionContext.RequestContext.Principal.Identity.Name;
                var isEntity = data is Entity;
                Entity entity = data as Entity;
                if (isEntity)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.Created = DateTime.UtcNow;
                    entity.Modified = DateTime.UtcNow;
                    entity.CreatedBy = username;
                    entity.ModifiedBy = username;
                }
                var isShopChild = data is ShopChild;
                if (isShopChild)
                {
                    var shopChild = data as ShopChild;
                    var manager = actionContext.Request.GetOwinContext().Get<ApplicationUserManager>();
                    var id = actionContext.RequestContext.Principal.Identity.GetUserId();
                    ApplicationUser user = manager.FindById(id);
                    shopChild.ShopId = user.ShopId;
                }
                
                ClientRequestModel client = new ClientRequestModel(actionContext) { UserName = username };
                string clientConnectionId = client.ConnectionId;
                Thread.SetData(Thread.GetNamedDataSlot("ConnectionId"), clientConnectionId);
                Trace.TraceInformation("Trace Request: {0}  clientConnectionId: {1}", client, clientConnectionId);
                string clientStr = client.ToString();
                logger.Debug("Client details: {0} ", clientStr);
                logger.Debug("EntitySaveObject: {1}", entity);
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data must not be empty");
            }

        }
    }
}