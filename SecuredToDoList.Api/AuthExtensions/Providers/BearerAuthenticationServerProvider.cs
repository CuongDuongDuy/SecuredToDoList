using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SecuredToDoList.Api.AuthExtensions.Repositories;

namespace SecuredToDoList.Api.AuthExtensions.Providers
{
    public class BearerAuthenticationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
            var authenticationRepository = new AuthenticationRepository(context.OwinContext);
            var user = await authenticationRepository.FindUserAsync(context.UserName, context.Password);

            if (user == null || !user.EmailConfirmed)
            {
                context.SetError("Invalid Grant", "The user name or password is incorrect, or email is not confirmed.");
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            var userRoles = await authenticationRepository.GetRolesAsync(user.Id);
            foreach (var role in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            context.Validated(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}