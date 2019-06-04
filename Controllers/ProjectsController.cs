using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JohnsBugTracker.Helpers;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace JohnsBugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        //GET: Projects
        //[Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        //public ActionResult MyIndex()
        //{
        //    var projectHelper = new ProjectHelper
        //    return View(projectHelper.ListUserProjects(userId));

        //}
        [Authorize]
        public ActionResult MyIndex()
        {
            return View(projectHelper.ListUserProjects(User.Identity.GetUserId()));
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);

            //what users occupy the role of devs?
            var developers = roleHelper.UsersInRole("Developer");
            var submitters = roleHelper.UsersInRole("Submitter");
            var projectManagers = roleHelper.UsersInRole("ProjectManager");

            //who is on project. use 4th parameter
            var devsOnProject = new List<string>();
            var subsOnProject = new List<string>();
            var pmOnProject = "";

            foreach (var user in projectHelper.UsersOnProject(project.Id))
            {
                var userRole = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
                switch(userRole)
                {
                    case "Developer":
                        devsOnProject.Add(user.Id);
                        break;
                    case "Submitter":
                        subsOnProject.Add(user.Id);
                        break;
                    case "ProjectManager":
                        pmOnProject = user.Id;
                        break;
                    default:
                        break;

                }

            }

            //viewbag properties to hold devs, subs, pm's
            ViewBag.Developers = new MultiSelectList(developers, "Id", "FullName", devsOnProject);
            ViewBag.Submitters = new MultiSelectList(submitters, "Id", "FullName", subsOnProject);
            ViewBag.ProjectManager = new SelectList(projectManagers, "Id", "FullName", pmOnProject);


            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project, List<string> Developers, List<string> Submitters, string ProjectManager)
        {
            if (ModelState.IsValid)
            {
                //unassign everyone from the project
                foreach(var user in projectHelper.UsersOnProject(project.Id).ToList())
                {
                    projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                //i want to add back to the project all the selected devs
                if(Developers != null)
                {
                    foreach(var userId in Developers)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //all selected subs
                if (Submitters != null)
                {
                    foreach (var userId in Submitters)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //projectmanager
                if (ProjectManager != null)
                {
                    projectHelper.AddUserToProject(ProjectManager, project.Id);
                }


                    db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
