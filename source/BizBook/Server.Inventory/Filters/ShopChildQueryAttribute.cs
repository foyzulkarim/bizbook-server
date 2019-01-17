using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Serilog;
using Server.Inventory.Models;

namespace Server.Inventory.Filters
{
    public class ShopChildQueryAttribute : ActionFilterAttribute
    {
        public static ILogger logger = Log.ForContext<ShopChildQueryAttribute>();

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {

            object appUser = null;
            bool tryGetValue = actionContext.Request.Properties.TryGetValue("AppUser", out appUser);

            if (tryGetValue && appUser is ApplicationUser user)
            {
                ClientRequestModel client =
                    new ClientRequestModel(actionContext) { UserName = user.UserName, ShopId = user.ShopId };

                string clientConnectionId = client.ConnectionId;
                Thread.SetData(Thread.GetNamedDataSlot("AppUser"), user);
                logger.Debug("Query Request: {@Client} {ConnectionId}", client, clientConnectionId);
                bool containsKey = actionContext.ActionArguments.ContainsKey("request");
                if (containsKey)
                {
                    dynamic request = actionContext.ActionArguments["request"];
                    request.ShopId = user.ShopId;
                    if (request.Keyword != null)
                    {
                        logger.Debug(
                            "Request model : {Username} requested {RawUrl} with values {@Request}",
                            user.UserName,
                            client.RawUrl,
                            request);
                    }
                }
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }


}