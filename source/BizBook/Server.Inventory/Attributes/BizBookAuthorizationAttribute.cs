using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Server.Inventory.Models;

namespace Server.Inventory.Attributes
{
    using System;
    using System.Linq;

    public class BizBookAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //return true;
            string bizbookserveridentity = ConfigurationManager.AppSettings["IdentityServer"];
            var request = actionContext.Request;
            string resourceName = request.RequestUri.AbsolutePath;
            var last = resourceName.Split(new string[] { "/api" }, StringSplitOptions.RemoveEmptyEntries).Last();
            resourceName = "/api" + last;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = request.Headers.Authorization;
                PermissionRequest permissionRequest = new PermissionRequest { Name = resourceName };
                string url = bizbookserveridentity + "api/Authorization/Authorize";
                var responseMessage = client.PostAsJsonAsync(url, permissionRequest).Result;
                string result = responseMessage.Content.ReadAsStringAsync().Result;

                bool isAuthorized = responseMessage.StatusCode == HttpStatusCode.OK;
                if (isAuthorized)
                {
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(result);
                    if (user != null)
                    {
                        actionContext.Request.Properties.Add("AppUser", user);
                        dynamic controller = actionContext.ControllerContext.Controller;
                        controller.AppUser = user;
                    }
                }

                return isAuthorized;
            }
        }
    }
}