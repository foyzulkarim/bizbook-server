namespace Server.Inventory.Handlers
{
    using System.Web.Http.ExceptionHandling;

    //https://stackoverflow.com/questions/16028919/catch-all-unhandled-exceptions-in-asp-net-web-api
    public class CustomExceptionHandler : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Serilog.Log.Logger.Error(context.Exception, "Unhandled exception occurred");
        }
    }
}