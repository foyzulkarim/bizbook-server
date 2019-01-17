namespace Server.Inventory.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class HmacSignatureFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestContent = actionContext.Request.Content;
            var jsonContent = requestContent.ReadAsStringAsync().Result;
            var byteContent = requestContent.ReadAsByteArrayAsync().Result;

            //if the request contains this, it's the verification request from Woocommerce
            //when the webhook is created so let it pass through so it can be verified
            if (!jsonContent.Contains("webhook_id"))
            {
                var requestSignature = actionContext.Request.Headers;

                //var bodyHash = HashHMAC("123", byteContent); //this is the shared key between Woo and custom API.  should be from config or database table.

                var signature = actionContext.Request.Headers.GetValues("x-wc-webhook-signature").FirstOrDefault();
                var source = actionContext.Request.Headers.GetValues("x-wc-webhook-source").FirstOrDefault();

                if (string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(source))
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }

                actionContext.Request.Properties.Add("Source", source);

                //if (bodyHash != signature.FirstOrDefault())
                //{
                //    throw new HttpResponseException(HttpStatusCode.Forbidden);
                //}
            }

            base.OnActionExecuting(actionContext);

        }

        private static string HashHMAC(string key, byte[] message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hash = new HMACSHA256(keyBytes);

            var computedHash = hash.ComputeHash(message);
            return Convert.ToBase64String(computedHash);
        }

    }
}