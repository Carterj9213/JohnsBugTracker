﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JohnsBugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }

        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        public bool Deleted { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 40 characters long.")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The description must be between 3 and 100 characters long.")]
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

      

        //Parents...
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        //Children...
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        public Ticket()
        {
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();
        }
    }
}