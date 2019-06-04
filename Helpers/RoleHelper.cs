using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JohnsBugTracker.Helpers
{
    public class RoleHelper
    {
        private UserManager<ApplicationUser> userManager = new
        UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new
        ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();
        public static ApplicationDbContext dB = new ApplicationDbContext();
        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }
        public bool IsEmailInRole(string email, string roleName)
        {
            var userId = userManager.FindByEmail(email).Id;
            return userManager.IsInRole(userId, roleName);
        }
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        public ICollection<string> ListEmailRoles(string email)
        {
            var userId = userManager.FindByEmail(email).Id;
            return userManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;

        }

        public static string GetUserImage(string userId)
        {
            var userImage = dB.Users.Find(userId).ProfilePic;
            if (string.IsNullOrEmpty(userImage))
            {
                userImage = "/images/avatar2.png";
            }
            return userImage;
        }
    }
}