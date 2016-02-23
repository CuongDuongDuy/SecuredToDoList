using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecuredToDoList.Api.Controllers
{
    public class BaseController : ApiController
    {
        private ClaimsPrincipal claimsPrincipal = null;

        public ClaimsPrincipal CurrentClaimsPrincipal
        {
            get { return claimsPrincipal ?? (claimsPrincipal = GetCurrentClaimPrincipal()); }
        }

        private ClaimsPrincipal GetCurrentClaimPrincipal()
        {
            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
            return principal ?? new NullClaimPrincipal();
        }

        protected string GetCurrentClaimValue(string claimType)
        {
            if (CurrentClaimsPrincipal.GetType() == typeof(NullClaimPrincipal))
            {
                return string.Empty;
            }
            var claim = CurrentClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType);
            return claim == null ? string.Empty : claim.Value;
        }
    }

    public class NullClaimPrincipal : ClaimsPrincipal
    {
        
    }
}
