using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SecuredToDoList.Api.Models;
using SecuredToDoList.Api.Repositories;

namespace SecuredToDoList.Api.Controllers
{
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private readonly AuthenticationRepository repo = null;

        public AccountsController()
        {
            repo = new AuthenticationRepository();
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
 
            var result = await repo.RegisterUser(userModel);
 
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
