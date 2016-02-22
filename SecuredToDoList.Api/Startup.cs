using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SecuredToDoList.Api;
using SecuredToDoList.Api.AuthExtensions.Managers;
using SecuredToDoList.Api.AuthExtensions.Models;
using SecuredToDoList.Api.AuthExtensions.Providers;
using SecuredToDoList.Api.Models;

[assembly: OwinStartup(typeof(Startup))]

namespace SecuredToDoList.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            ConfigureOAuth(appBuilder);

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new BearerAuthenticationServerProvider()
            };

            app.CreatePerOwinContext<AuthDbContext>(AuthDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
