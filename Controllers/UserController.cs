using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace JohnsBugTracker.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myUserViewModel = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ProfilePic = user.ProfilePic
            };


             return View(myUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserViewModel user, HttpPostedFileBase avatar)
        {
            var newUser = db.Users.Find(user.Id);

            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.DisplayName = user.DisplayName;
            newUser.Email = user.Email;
            newUser.UserName = user.Email;

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
       
    }
}