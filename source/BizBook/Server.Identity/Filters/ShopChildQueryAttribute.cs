using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Serilog;
using Server.Identity.Models;

namespace Server.Identity.Filters
{
    public class ShopChildQueryAttribute : ActionFilterAttribute
    {
        public static ILogger logger = Log.ForContext<ShopChildQueryAttribute>();

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {        
            ApplicationUserManager manager = actionContext.Request.GetOwinContext().Get<ApplicationUserManager>();

            string username = actionContext.RequestContext.Principal.Identity.Name;
            
            if (!string.IsNullOrWhiteSpace(username))
            {
                ApplicationUser user = manager.FindByName(username);
                if (user != null)
                {
                    actionContext.Request.Properties.Add("AppUser", user);
                    dynamic controller = actionContext.ControllerContext.Controller;
                    controller.AppUser = user;
                    ClientRequestModel client = new ClientRequestModel(actionContext)
                    {
                        UserName = user.UserName,
                        ShopId = user.ShopId
                    };

                    string clientConnectionId = client.ConnectionId;
                    Thread.SetData(Thread.GetNamedDataSlot("AppUser"), user);
                    Trace.TraceInformation("Trace Query Request: {0} {1}", client, clientConnectionId);
                    string clientStr = client.ToString();
                    logger.Debug("Query Request: {ClientStr} {ConnectionId}", clientStr, clientConnectionId);
                    bool containsKey = actionContext.ActionArguments.ContainsKey("request");
                    if (containsKey)
                    {
                        dynamic request = actionContext.ActionArguments["request"];
                        request.ShopId = user.ShopId;
                        if (request.Keyword != null)
                        {
                            logger.Debug("Request model : {Username} requested {RawUrl} with values {@Request}", username, client.RawUrl, request);
                        }
                    }
                }
            }
           

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }


}