using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JohnsBugTracker.Models;
using Microsoft.AspNet.Identity;
using JohnsBugTracker.Helpers;

namespace JohnsBugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private TicketHistoryHelper historyHelper = new TicketHistoryHelper();
        

        // GET: Tickets
        [Authorize (Roles = "Admin, ProjectManager")]
        public ActionResult Index()
        {
            //var tickets= TicketHelper .where(t.)
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        [Authorize]
        [Authorize (Roles = "ProjectManager, Developer, Submitter")]
        public ActionResult MyIndex()
        {
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var myTickets = new List<Ticket>();
            switch (myRole)
            {
                case "Submitter":
                    myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                case "Developer":
                    myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;               
                case "ProjectManager":
                    myTickets = db.Users.Find(userId).Projects.SelectMany(p => p.Tickets).ToList();
                    break;
                default:
                    break;
            }
            return View("Index", myTickets);
        }

        //[Authorize]
        //public ActionResult AssignUsers(int id)
        //{        
        //    var userHelper = new UserHelper();
        //    var developers = userHelper.GetUsersOnProjectByRole(db.Tickets.Find(id).ProjectId, "Developer");
        //    ViewBag.Developers = new SelectList(developers, "Id", "FullName", Ticket.AssignedToUserId);
        //    ViewBag.TicketId = id;
        //    return View();
        //}

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        [Authorize]
        public ActionResult Create(int? projectId)
        {
            var myTicket = new Ticket();

            if (projectId == null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
                myTicket.ProjectId = -1;

            }
            else
            {
                myTicket.ProjectId = (int)projectId;
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(myTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        [Authorize (Roles = "Admin, ProjectManager, Developer, Submitter")]
        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            //we need to determine whether to kick person out or let view render
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var isAuthorized = false;
            switch (myRole)
            {
                case "Submitter":
                    isAuthorized = (ticket.OwnerUserId == userId);
                    break;
                case "Developer":
                    isAuthorized = (ticket.AssignedToUserId == userId);
                    break;
                case "Admin":
                    isAuthorized = true;
                    break;
                case "ProjectManager":
                    isAuthorized = db.Users.Find(userId).Projects.Select(p => p.Id).Contains(ticket.ProjectId);
                    break;
                default: 
                    break;
            }
            if (!isAuthorized)
            {
                return RedirectToAction("Login", "Home");
            }

            var developers = new List<ApplicationUser>();
            var usersOnProject = projectHelper.UsersOnProject(ticket.ProjectId);
            foreach (var user in usersOnProject)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                {
                    developers.Add(user);
                }
            }


            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.AssignedToUserId = new SelectList(developers, "Id", "Email", ticket.AssignedToUserId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {   //There are times when I need to compare the new value of a Ticket property against
                //the old value. When I have to do this I need to go out to my DB and grab the Ticket
                //before the db.SaveChanges in order to have a copy of the old Ticket. 
                //Now I can compare the old ticket to the new incoming "ticket".

                //var oldAssignedToUserId = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id).AssignedToUserId;
                //var oldTicketStatusId = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id).TicketStatusId;
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                notificationHelper.TriggerAssignmentNotifications(ticket.Id, oldTicket.AssignedToUserId, ticket.AssignedToUserId);

                //manually change the status
                if(oldTicket.TicketStatusId == ticket.TicketStatusId)
                {
                    ticket.TicketStatusId = ticketHelper.GetNewTicketStatus(oldTicket.AssignedToUserId, ticket.AssignedToUserId);
                }

                //call out ticket history helper to record any meaniful changes in property vales
                historyHelper.RecordTicketChanges(oldTicket, ticket);

     
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
