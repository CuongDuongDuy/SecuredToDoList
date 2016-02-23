using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SecuredToDoList.Api;
using SecuredToDoList.Api.AuthExtensions.Configs;
using SecuredToDoList.Api.AuthExtensions.Managers;
using SecuredToDoList.Api.AuthExtensions.Models;
using SecuredToDoList.Api.AuthExtensions.Providers;

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
