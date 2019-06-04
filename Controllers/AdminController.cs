using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JohnsBugTracker.Helpers;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace JohnsBugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private RoleHelper roleHelper = new RoleHelper();

        private ProjectHelper projectHelper = new ProjectHelper();

        //GET: ADMIN
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ManageRoles()
        {
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Email");
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(string users, string roles)
        {
            //I want to ensure who i select occupies that role
            

            foreach(var role in roleHelper.ListUserRoles(users))
            {
                roleHelper.RemoveUserFromRole(users, role);
            }
            //newly selected role to user
            roleHelper.AddUserToRole(users, roles);

            return RedirectToAction("UserIndex", "Admin");

        }
        [Authorize]
        public ActionResult ManageMyRole(string email)
        {
           
            var myCurrentRole = roleHelper.ListEmailRoles(email).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name", myCurrentRole);
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "Name");
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageMyRole(string email, string roles)
        {
            var userId = db.Users.FirstOrDefault(u => u.Email == email).Id;

            foreach (var role in roleHelper.ListUserRoles(userId))
            {             
                roleHelper.RemoveUserFromRole(userId, role);
            }
            //newly selected role to user
            roleHelper.AddUserToRole(userId, roles);

            return RedirectToAction("UserIndex", "Admin");
        }

        [Authorize(Roles ="Admin")]
        public ActionResult UserIndex()
        {
            var users = db.Users.Select(u => new UserViewModel
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                Email = u.Email

            }).ToList();

            return View(users);
        }




    }
}