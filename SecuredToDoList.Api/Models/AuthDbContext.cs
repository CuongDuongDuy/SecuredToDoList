using Microsoft.AspNet.Identity.EntityFramework;

namespace SecuredToDoList.Api.Models
{
    public class AuthDbContext:IdentityDbContext<IdentityUser>
    {
        public AuthDbContext()
            : base("AuthenticationDatabase")
        {
            
        }
    }
}