﻿using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
        [ValidateInput(false)]
	public class TaskController : Controller
	{
	    [HttpGet]
        [Route("")]
	    public ActionResult Index()
	    {
            using (var database = new TodoListDbContext())
            {
                var tasks = database.Tasks
                    .ToList();

                return View(tasks);
            }
        }

        [HttpGet]
        [Route("create")]
        public ActionResult Create()
		{
            return View();
        }

		[HttpPost]
		[Route("create")]
        [ValidateAntiForgeryToken]
		public ActionResult Create(Task task)
		{
            if (ModelState.IsValid)
            {
                using (var database = new TodoListDbContext())
                {

                    database.Tasks.Add(task);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(task);
        }

		[HttpGet]
		[Route("delete/{id}")]
        public ActionResult Delete(int? id)
		{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new TodoListDbContext())
            {
                var task = database.Tasks.Where(t => t.Id == id).First();

                if (task == null)
                {
                    return HttpNotFound();
                }

                return View(task);
            }
        }

		[HttpPost]
		[Route("delete/{id}")]
        [ValidateAntiForgeryToken]
		public ActionResult DeleteConfirm(int? id)
		{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new TodoListDbContext())
            {
                var task = database.Tasks.Where(t => t.Id == id).First();

                if (task == null)
                {
                    return HttpNotFound();
                }

                database.Tasks.Remove(task);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
        }
	}
}