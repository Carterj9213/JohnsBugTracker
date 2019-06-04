using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JohnsBugTracker.Models;

namespace JohnsBugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public void RecordTicketChanges(Ticket oldTicket, Ticket newTicket)
        {
            //compare the oldticket property values to the new ticket properties
            //if diff then we add new ticket history

           
           
            //public int TicketTypeId { get; set; }
            //public int TicketPriorityId { get; set; }
            //public int TicketStatusId { get; set; }

            //public string OwnerUserId { get; set; }
            //public string AssignedToUserId { get; set; }

           // public string Title { get; set; }
           


            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                AddTicketHistory(newTicket.Id, "TicketTypeId", oldTicket.TicketTypeId.ToString(), oldTicket.TicketTypeId.ToString());
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                AddTicketHistory(newTicket.Id, "TicketPriorityId", oldTicket.TicketPriorityId.ToString(), oldTicket.TicketPriorityId.ToString());
            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                AddTicketHistory(newTicket.Id, "TicketStatusId", oldTicket.TicketStatusId.ToString(), oldTicket.TicketStatusId.ToString());
            }

            if (oldTicket.OwnerUserId != newTicket.OwnerUserId)
            {
                AddTicketHistory(newTicket.Id, "OwnerUserId", oldTicket.OwnerUserId, oldTicket.OwnerUserId);
            }

            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                AddTicketHistory(newTicket.Id, "AssignedToUserId", oldTicket.AssignedToUserId, oldTicket.AssignedToUserId);
            }

            if (oldTicket.Title != newTicket.Title)
            {
                AddTicketHistory(newTicket.Id, "Title", oldTicket.Title.ToString(), oldTicket.Title.ToString());
            }

            //if (oldTicket.Title != newTicket.Title)
            //{

            //}

        }

        public void AddTicketHistory(int ticketId, string propertyName, string oldValue, string newValue)
        {
            var newTicketHistory = new TicketHistory
            {
                Property = propertyName,
                OldValue = oldValue,
                NewValue = newValue,
                Changed = DateTime.Now,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                TicketId = ticketId
            };

            db.TicketHistories.Add(newTicketHistory);
            db.SaveChanges();
        }

        public static string GetHistoryInfo(string Id, string property)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return "";
            }
            var info = Id;
            switch (property)
            {
                case "AssignedToUserId":
                case "OwnerUserId":
                    info = db.Users.Find(Id).Email;
                    break;
                case "TicketStatusId":
                    info = db.TicketStatuses.Find(Convert.ToInt32(Id)).Name;
                    break;
                case "TicketPriorityId":
                    info = db.TicketPriorities.Find(Convert.ToInt32(Id)).Name;
                    break;
                case "TicketTypeId":
                    info = db.TicketTypes.Find(Convert.ToInt32(Id)).Name;
                    break;
                default:
                    break;
            }
            return info;
        }

    }
       
}