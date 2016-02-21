using Microsoft.AspNet.Identity.EntityFramework;

namespace SecuredToDoList.Api.AuthExtensions.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CallName { get; set; }
    }
}