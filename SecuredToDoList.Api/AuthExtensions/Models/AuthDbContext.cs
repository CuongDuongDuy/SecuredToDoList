using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecuredToDoList.Api.AuthExtensions.Configs;

namespace SecuredToDoList.Api.AuthExtensions.Models
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext()
            : base("AuthenticationDatabase")
        {

        }

        static AuthDbContext()
        {
            Database.SetInitializer(new AuthDbInitializer());
        }

        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }
    }

}