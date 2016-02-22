using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SecuredToDoList.Api.AuthExtensions.Models;
using SecuredToDoList.Api.AuthExtensions.Repositories;

namespace SecuredToDoList.Api.Controllers
{
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private AuthenticationRepository authenticationRepository;

        public AuthenticationRepository AuthenticationRepository
        {
            get
            {
                return authenticationRepository ?? new AuthenticationRepository(Request.GetOwinContext());
            }
            private set { authenticationRepository = value; }
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            var result = await AuthenticationRepository.RegisterUserAsync(userModel);
 
            var errorResult = GetErrorResult(result);
 
            return errorResult ?? Ok();
        }
 
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded) return null;
            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
 
            if (ModelState.IsValid)
            {
                return BadRequest();
            }
 
            return BadRequest(ModelState);
        }
    }
}
