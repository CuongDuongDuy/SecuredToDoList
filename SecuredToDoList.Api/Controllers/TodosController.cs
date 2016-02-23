using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using SecuredToDoList.Api.Attributes;
using SecuredToDoList.Api.Models;

namespace SecuredToDoList.Api.Controllers
{
    [RoutePrefix("api/ToDos")]
    public class TodosController : BaseController
    {
        private readonly ApplicationDbContext db;

        public TodosController()
        {
            db = new  ApplicationDbContext();
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<TodoItem> Index()
        {
            var userEmail = GetCurrentClaimValue(ClaimTypes.Email);
            var todos = db.ToDoItems.Where(x => x.IsPublic || x.AttendeeEmail == userEmail).ToList();
            return todos;
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<TodoItem> Details(Guid id)
        {
            var userEmail = GetCurrentClaimValue(ClaimTypes.Email);
            var todo = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id && (x.IsPublic || x.AttendeeEmail == userEmail));
            return todo;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(TodoItemEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = new TodoItem
            {
                Title = model.Title,
                IsDone = model.IsDone,
                IsPublic = model.IsPublic,
                AttendeeEmail = model.AttendeeEmail
            };

            db.ToDoItems.Add(todo);
            await db.SaveChangesAsync();
            return Ok(todo.Id);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Edit(Guid id, TodoItemEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Title = model.Title;
            todo.IsDone = model.IsDone;
            todo.IsPublic = model.IsPublic;
            todo.AttendeeEmail = model.AttendeeEmail;

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ClaimAuthorized(RoleNames = "admin")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            var todoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id);
            db.ToDoItems.Remove(todoItem);
            await db.SaveChangesAsync();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}