using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
                return authenticationRepository ?? (authenticationRepository =  new AuthenticationRepository(Request.GetOwinContext()));
            }
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
            var identityResult = await AuthenticationRepository.RegisterUserAsync(userModel);

            if (identityResult.Succeeded)
            {
                var user = await AuthenticationRepository.FindUserAsync(userModel.Email);
                var token = await AuthenticationRepository.GetEmailConfirmationCodeAsync(user.Id);
                var callbackLink = Url.Link("ConfirmEmail", new {userId = user.Id, code = token});
                return Ok(callbackLink);
            }
            return BadRequest(identityResult.Errors.Aggregate(string.Empty, (current, error) => current + Environment.NewLine + error));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId)|| string.IsNullOrEmpty(code))
            {
                return BadRequest("Unvalid parameters");
            }
            var identityResult = await AuthenticationRepository.ConfirmEmail(userId, code);
            if (identityResult.Succeeded)
            {
                return Ok();
            }
            return BadRequest(identityResult.Errors.Aggregate(string.Empty, (current, error) => current + Environment.NewLine + error));
        }

    }
}
