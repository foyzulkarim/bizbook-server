using System.Web;

using System.Web.Mvc;
using System.Web.Routing;

namespace Server.Inventory
{
    using System.Configuration;
    using System.Web.Http.ExceptionHandling;
    using LoggingService;
    using Server.Inventory.Handlers;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            string name = ConfigurationManager.AppSettings["LoggingDefaults"];
            SeriLogHelper.ConfigureLoggingDefaults(name);
            System.Web.Http.GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = System.Web.Http.IncludeErrorDetailPolicy.Always;
            System.Web.Http.GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new CustomExceptionHandler());
            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultSchedulerConnection").UseDashboardMetric(SqlServerStorage.ActiveConnections);

            //SampleScheduler.Start();
        }

    }
}
