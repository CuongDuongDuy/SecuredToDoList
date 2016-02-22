using System.Data.Entity;
using SecuredToDoList.Api.AuthExtensions.Models;

namespace SecuredToDoList.Api.AuthExtensions.Configs
{
    public class AuthDbInitializer : DropCreateDatabaseIfModelChanges<AuthDbContext>
    {
    }
}