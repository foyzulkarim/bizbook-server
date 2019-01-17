using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Server.Identity;
using Server.Identity.Providers;

[assembly: OwinStartup(typeof(Startup))]

namespace Server.Identity
{
    using LoggingService;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
            app.Map("/signalr", map =>
            {
                //CorsPolicyProvider provider = new EnableCorsAttribute("*", "*", "*");
                CorsOptions options = CorsOptions.AllowAll;
                //options.PolicyProvider =
                map.UseCors(options);

                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
                {
                    Provider = new OAuthTokenProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = GlobalHost.DependencyResolver
                };
                map.RunSignalR(hubConfiguration);
            });

            string name = ConfigurationManager.AppSettings["LoggingDefaults"];
            SeriLogHelper.ConfigureLoggingDefaults(name);
            CacheConfig.Register();
        }
    }
}
