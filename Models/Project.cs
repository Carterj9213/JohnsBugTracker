using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JohnsBugTracker.Models
{
    public class Project
    {

        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "The name must be between 4 and 40 characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "The description must be between 4 and 60 characters long.")]
        public string Description { get; set; }


        //kid

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();

        }


    }
}