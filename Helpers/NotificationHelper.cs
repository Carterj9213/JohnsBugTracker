using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace JohnsBugTracker.Helpers
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public void TriggerAssignmentNotifications(int ticketId, string oldDeveloper, string newDeveloper)
        {
            //setup a few variables that tell me which is afoot
            var newAssignment = string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper);
            var unAssignment = !string.IsNullOrEmpty(oldDeveloper) && string.IsNullOrEmpty(newDeveloper);
            var reAssignment = !string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper) && (oldDeveloper != newDeveloper);

            if (newAssignment)
            {
                AddAssignmentNotification(ticketId, newDeveloper);
            }
            else if (unAssignment)
            {
                AddUnAssignmentNotification(ticketId, oldDeveloper);
            }
            else if (reAssignment)
            {
                AddAssignmentNotification(ticketId, newDeveloper);
                AddUnAssignmentNotification(ticketId, oldDeveloper);
            }

        }

        private void AddAssignmentNotification(int ticketId, string newDeveloper)
        {
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                TicketId = ticketId,
                Unread = true,
                UserId = newDeveloper,
                NotificationBody = $"You have been assigned to Ticket: {ticketId}."

            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }
        private void AddUnAssignmentNotification(int ticketId, string oldDeveloper)
        {
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                TicketId = ticketId,
                Unread = true,
                UserId = oldDeveloper,
                NotificationBody = $"You have been unassigned to Ticket: {ticketId}."

            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();

        }
        
        public static List<TicketNotification> GetUnreadNotifications()
        {
            var list = new List<TicketNotification>();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                list = db.TicketNotifications.Where(t => t.UserId == userId && t.Unread).ToList();
            }
            return list;
        }





    }
}