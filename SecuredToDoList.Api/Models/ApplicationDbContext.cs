using System.Data.Entity;

namespace SecuredToDoList.Api.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext():base("ToToListDatabase")
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }
        public IDbSet<TodoItem> ToDoItems { get; set; }

        public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = true,
                    Title = "Public Job 1",
                    Worker = "account1"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = true,
                    Title = "Public Job 2",
                    Worker = "account1"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = false,
                    Title = "Private Job 1",
                    Worker = "account1"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = false,
                    Title = "Private Job 2",
                    Worker = "account1"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = true,
                    Title = "Public Job 1",
                    Worker = "account2"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = true,
                    Title = "Public Job 2",
                    Worker = "account2"
                });
                context.ToDoItems.Add(new TodoItem
                {
                    IsDone = false,
                    IsPublic = false,
                    Title = "Private Job 1",
                    Worker = "account2"
                });
                context.SaveChanges();
            }
        }
    }
}