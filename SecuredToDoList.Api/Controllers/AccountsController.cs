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
        private readonly AuthenticationRepository authenticationRepository;

        public AccountsController()
        {
            authenticationRepository = new AuthenticationRepository();
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
 
            var result = await authenticationRepository.RegisterUserAsync(userModel);
 
            var errorResult = GetErrorResult(result);
 
            return errorResult ?? Ok();
        }
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
 
            base.Dispose(disposing);
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
