using System.Web.Http;
using Microsoft.Owin;
using Owin;
using SecuredToDoList.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace SecuredToDoList.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);

        }
    }
}
