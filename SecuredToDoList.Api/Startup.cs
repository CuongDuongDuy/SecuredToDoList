using System.Web.Http;
using Microsoft.Owin;
using Owin;
using SecuredToDoList.Api;
using SecuredToDoList.Api.AuthExtensions.Configs;

[assembly: OwinStartup(typeof(Startup))]

namespace SecuredToDoList.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            OAuthConfig.ConfigureOAuth(appBuilder);

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }

        
    }
}
