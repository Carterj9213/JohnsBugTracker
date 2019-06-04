using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JohnsBugTracker.Models;

namespace JohnsBugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private RoleHelper rolehelper = new RoleHelper();
        public int GetNewTicketStatus(string oldDeveloper, string newDeveloper)
        {
            var newAssignment = string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper);
            var unAssignment = !string.IsNullOrEmpty(oldDeveloper) && string.IsNullOrEmpty(newDeveloper);
            var reAssignment = !string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper) && oldDeveloper != newDeveloper;

            var statusId = -1;

            if (newAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id;
            }
            else if(unAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;
            }
            else if (reAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
            }
            else 
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unknown").Id;

            }
            return statusId;   
        }
    }
}