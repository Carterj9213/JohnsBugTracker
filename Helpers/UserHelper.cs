using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JohnsBugTracker.Models;

namespace JohnsBugTracker.Helpers
{
    public class UserHelper
    {
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        public List<ApplicationUser> GetUsersOnProjectByRole(int projectId, string role)
        {
            var users = new List<ApplicationUser>();
            var projectUsers = projectHelper.UsersOnProject(projectId);
            foreach(var user in projectUsers)
            {
                if (roleHelper.IsUserInRole(user.Id, role))
                {
                    users.Add(user);
                }

            }
            return users;
            
        }

    }
}