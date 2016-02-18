using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecuredToDoList.Api.Models;

namespace SecuredToDoList.Api.Repositories
{
    public class AuthenticationRepository: IDisposable
    {
        private readonly AuthDbContext ctx;
 
        private readonly UserManager<IdentityUser> userManager;
 
        public AuthenticationRepository()
        {
            ctx = new AuthDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx));
        }
 
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };
 
            var result = await userManager.CreateAsync(user, userModel.Password);
 
            return result;
        }
 
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await userManager.FindAsync(userName, password);
 
            return user;
        }
 
        public void Dispose()
        {
            ctx.Dispose();
            userManager.Dispose();
 
        }
    }
}