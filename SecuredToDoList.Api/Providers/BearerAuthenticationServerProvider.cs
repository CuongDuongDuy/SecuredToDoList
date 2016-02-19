using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using SecuredToDoList.Api.Repositories;

namespace SecuredToDoList.Api.Providers
{
    public class BearerAuthenticationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

            using (var repo = new AuthenticationRepository())
            {
                var user = await repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("Invalid Grant", "The user name or password is incorrect.");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                foreach (var role in user.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleId));
                }
                context.Validated(identity);
            }
        }
    }
}