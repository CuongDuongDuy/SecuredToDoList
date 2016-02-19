using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecuredToDoList.Api.Models;

namespace SecuredToDoList.Api.Repositories
{
    public class AuthenticationRepository: IDisposable
    {
        private readonly AuthDbContext ctx;
 
        private readonly UserManager<IdentityUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager; 
 
        public AuthenticationRepository()
        {
            ctx = new AuthDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
        }
 
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            await roleManager.CreateAsync(new IdentityRole("SuperUser"));
            var user = new IdentityUser
            {
                UserName = userModel.UserName
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
 
        public void Dispose()
        {
            ctx.Dispose();
            userManager.Dispose();
 
        }

        public IEnumerable<string> GetRoleIds(IEnumerable<string> roleNamesSplit)
        {
            return roleManager.Roles.Where(x => roleNamesSplit.Contains(x.Name)).Select(x => x.Name);
        }
    }
}