using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;
            public ToDoController(ToDoContext context)
        {
            this.context = context;    
        }
        //GET//
        public async Task<ActionResult> Index()
        {
  
            IQueryable<TodoList> items = from i in context.ToDoLists orderby i.Id select i;
            List<TodoList> todoList = await items.ToListAsync();
            return View(todoList);
        }
        //Get/Create/Todolist
        

        public IActionResult Create ()=>View();

        //post /todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if(ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Item has been Addedd Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //Edit/PUT
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoLists.FindAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            
            return View(item);
        }
        //POST/edit
        //post /todo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Item has been Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //Delete
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoLists.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "This Item is no more";
            }
            else
            {
                context.ToDoLists.Remove(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Item has been Deleted Successfully";
            }

            return RedirectToAction("Index");
        }
    }
}
