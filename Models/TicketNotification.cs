using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnsBugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string NotificationBody { get; set; }
        public DateTime Created { get; set; }
        public bool Unread { get; set; }


        //parent

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}