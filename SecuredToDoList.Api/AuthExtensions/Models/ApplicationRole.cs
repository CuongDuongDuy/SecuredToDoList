using Microsoft.AspNet.Identity.EntityFramework;

namespace SecuredToDoList.Api.AuthExtensions.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string name, string description) : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}