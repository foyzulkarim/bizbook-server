using System;
using System.Web;
using System.Web.Http.Controllers;

namespace Server.Inventory.Models
{
    [Serializable]
    public class ClientRequestModel
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string RawUrl { get; set; }

        public string MobileDeviceModel { get; set; }

        public bool IsMobileDevice { get; set; }

        public string UserHostAddress { get; set; }

        public string ShopId { get; set; }

        public ClientRequestModel(HttpActionContext actionContext)
        {
            HttpRequestBase request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
            var browser = request.Browser;
            BrowserName = browser.Browser;
            Platform = browser.Platform;
            RawUrl = request.RawUrl;
            UserHostAddress = request.UserHostAddress;
            IsMobileDevice = browser.IsMobileDevice;
            MobileDeviceModel = browser.MobileDeviceModel;
            ConnectionId = request.Headers["ConnectionId"];
        }

    }
}