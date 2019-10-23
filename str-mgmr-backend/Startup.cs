using System;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(str_mgmr_backend.Startup))]

namespace str_mgmr_backend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            AreaRegistration.RegisterAllAreas();
            app.UseCors(CorsOptions.AllowAll);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = GlobalHost.DependencyResolver,
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
