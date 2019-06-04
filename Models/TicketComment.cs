using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JohnsBugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }    
       
        public string CommentBody { get; set; }
        public DateTimeOffset Created { get; set; }

      
        //parent

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }

        
    }
}