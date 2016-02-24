using System;
using System.Data.Entity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SecuredToDoList.Api.AuthExtensions.Managers;
using SecuredToDoList.Api.AuthExtensions.Models;
using SecuredToDoList.Api.AuthExtensions.Providers;

namespace SecuredToDoList.Api.AuthExtensions.Configs
{
    public class AuthDbInitializer : DropCreateDatabaseIfModelChanges<AuthDbContext>
    {
    }

    public class OAuthConfig
    {

        public static void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new BearerAuthenticationServerProvider()
            };

            app.CreatePerOwinContext(AuthDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}