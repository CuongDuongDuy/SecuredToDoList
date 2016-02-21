using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SecuredToDoList.Api.AuthExtensions.Managers;
using SecuredToDoList.Api.AuthExtensions.Models;

namespace SecuredToDoList.Api.AuthExtensions.Repositories
{
    public class AuthenticationRepository
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationRoleManager roleManager;
 
        public AuthenticationRepository()
        {
            userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
 
        public async Task<IdentityResult> RegisterUserAsync(UserModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.UserName,
                CallName = userModel.CallName
            };
            
            var identity = await userManager.CreateAsync(user, userModel.Password);
            foreach (var userRole in userModel.UserRoles.Split(','))
            {
                await userManager.AddToRoleAsync(user.Id, userRole.Trim());
            }
            return identity;
        }
 
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await userManager.FindAsync(userName, password);
            return user;
        }
    }
}