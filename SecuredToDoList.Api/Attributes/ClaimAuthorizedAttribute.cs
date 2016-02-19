using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SecuredToDoList.Api.Attributes
{
    public class ClaimAuthorizedAttribute : AuthorizationFilterAttribute
    {
        private static readonly string[] EmptyArray = new string[0];
        private IEnumerable<string> roleNamesSplit = EmptyArray;
        private IEnumerable<string> userLoginsSplit = EmptyArray;
        private string roleNames;
        private string userLogins;
        public string RoleNames
        {
            get { return roleNames ?? string.Empty; }
            set
            {
                roleNames = value;
                roleNamesSplit = SplitString(value);
            }
        }

        public string UserLogins
        {
            get
            {
                return userLogins ?? string.Empty;
            }
            set
            {
                userLogins = value;
                this.userLoginsSplit = SplitString(value);
            }
        }

        internal IEnumerable<string> SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return EmptyArray;
            var result = original.Split(',').Select(value => value.Trim());
            return result;
        }

        internal bool IsAuthorized(ClaimsPrincipal principal)
        {
            if (principal == null || principal.Claims == null )
                return false;
            var userLogin = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            if (userLogin == null)
            {
                return false;
            }
            var roles = principal.Claims.Where(x => x.Type == ClaimTypes.Role);
            var rolesArray = roles as Claim[] ?? roles.ToArray();
            if (!rolesArray.Any())
            {
                return false;
            }
            return rolesArray.Any(role => roleNamesSplit.Contains(role.Value)) || userLoginsSplit.Any(user => user == userLogin.Value);
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.Request.GetRequestContext().Principal as ClaimsPrincipal;
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated || !IsAuthorized(principal))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            return Task.FromResult<object>(null);
        }
    }
}
