using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Server.Inventory;

[assembly: OwinStartup(typeof(Startup))]

namespace Server.Inventory
{
    using System.Configuration;

    using LoggingService;

    using Microsoft.Owin.Security.OAuth;
    
    using Server.Inventory.Providers;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            //ConfigureAuth(app);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                                                 {
                                                     Provider = new OAuthTokenProvider()
                                                 });


            app.MapSignalR();
            string name = ConfigurationManager.AppSettings["LoggingDefaults"];
            SeriLogHelper.ConfigureLoggingDefaults(name);
            //GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultSchedulerConnection");
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
            
        }
    }
}
