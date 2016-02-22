using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SecuredToDoList.Api.AuthExtensions.Managers;
using SecuredToDoList.Api.AuthExtensions.Models;

namespace SecuredToDoList.Api.AuthExtensions.Repositories
{
    public class AuthenticationRepository
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationRoleManager roleManager;
        private readonly IAuthenticationManager authenticationManager;
 
        public AuthenticationRepository(IOwinContext owinContext)
        {
            userManager = owinContext.GetUserManager<ApplicationUserManager>();
            roleManager = owinContext.Get<ApplicationRoleManager>();
            authenticationManager = owinContext.Authentication;
        }
 
        public async Task<IdentityResult> RegisterUserAsync(UserModel userModel)
        {
            var user = new ApplicationUser
            {
                UserName = userModel.UserName,
                DisplayName = userModel.DisplayName,
                Email =  userModel.Email
            };
            
            var identity = await userManager.CreateAsync(user, userModel.Password);

            foreach (var userRole in userModel.UserRoles.Split(','))
            {
                if (roleManager.FindByName(userRole) == null)
                {
                    await roleManager.CreateAsync(new ApplicationRole(userRole, userRole));
                }
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