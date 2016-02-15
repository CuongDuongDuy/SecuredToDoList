using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SecuredToDoList.Api.Models;

namespace SecuredToDoList.Api.Controllers
{
    public class TodosController : ApiController
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
            var todos = db.ToDoItems.ToList();
            return todos;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<TodoItem> Details(Guid id)
        {
            var todo = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id);
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
                Worker = model.Worker
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
            todo.Worker = model.Worker;

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
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