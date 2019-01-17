namespace Server.Identity.Helper
{
    public class ApplicationInsightHelpers
    {
    }

    //public class RequestBodyInitializer : ITelemetryInitializer
    //{
    //    public void Initialize(ITelemetry telemetry)
    //    {
    //        var requestTelemetry = telemetry as RequestTelemetry;
    //        if (requestTelemetry != null && (requestTelemetry.HttpMethod == HttpMethod.Post.ToString() || requestTelemetry.HttpMethod == HttpMethod.Put.ToString()))
    //        {
    //            using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
    //            {
    //                string requestBody = reader.ReadToEnd();
    //                requestTelemetry.Properties.Add("body", requestBody);
    //            }
    //        }
    //    }
    //}
}