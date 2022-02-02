using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models; //added for access to DTO's
using ToDoAPI.DATA.EF; //added for access to EF
using System.Web.Http.Cors; //added for access to modify the CORS for this controller specifically

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET - READ
        //api/todo
        public IHttpActionResult GetTodos()
        {
            List<ToDoViewModel> todos = db.TodoItems.Include("Category").Select(x => new ToDoViewModel()
            {
                TodoId = x.TodoId,
                Action = x.Action,
                Done = x.Done,
                CategoryId = x.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = x.Category.CategoryId,
                    Name = x.Category.Name,
                    Description = x.Category.Description
                }
            }).ToList<ToDoViewModel>();

            if (todos.Count == 0)
            {
                return NotFound();
            }

            return Ok(todos);
        }//end GetTodos

        //GET - READ
        //api/todo/id
        public IHttpActionResult GetTodos(int id)
        {
            ToDoViewModel todo = db.TodoItems.Include("Category").Where(x => x.TodoId == id).Select(x => new ToDoViewModel()
            {
                //Copy the assignments from above
                TodoId = x.TodoId,
                Action = x.Action,
                Done = x.Done,
                CategoryId = x.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = x.Category.CategoryId,
                    Name = x.Category.Name,
                    Description = x.Category.Description
                }
            }).FirstOrDefault();

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }//end GetTodos

        //POST - CREATE
        //api/todos (HttpPost)
        public IHttpActionResult PostTodo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem newTodoItem = new TodoItem()
            {
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            db.TodoItems.Add(newTodoItem);
            db.SaveChanges();

            return Ok(newTodoItem);
        }//end PostTodo

        //PUT - EDIT
        //api/todos (HttpPut)
        public IHttpActionResult PutTodo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem existingTodoItem = db.TodoItems.Where(x => x.TodoId == todo.TodoId).FirstOrDefault();

            if (existingTodoItem != null)
            {
                existingTodoItem.TodoId = todo.TodoId;
                existingTodoItem.Action = todo.Action;
                existingTodoItem.Done = todo.Done;
                existingTodoItem.CategoryId = todo.CategoryId;

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutTodo

        //DELETE - DELETE
        //api/todos/id (HttpDelete)
        public IHttpActionResult DeleteTodo(int id)
        {
            TodoItem todoItem = db.TodoItems.Where(x => x.TodoId == id).FirstOrDefault();

            if (todoItem != null)
            {
                db.TodoItems.Remove(todoItem);
                db.SaveChanges();

                return Ok();
            }

            else
            {
                return NotFound();
            }
        }//end DeleteTodo

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }//end class
}//end namespace
