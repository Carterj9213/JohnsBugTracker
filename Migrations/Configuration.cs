namespace JohnsBugTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using JohnsBugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<JohnsBugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(JohnsBugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            //I want to write some code that allows me to introduce a few "roles" into our application
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "Jc56Wrestling@Yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Jc56Wrestling@Yahoo.com",
                    UserName = "Jc56Wrestling@Yahoo.com",
                    FirstName = "John",
                    LastName = "Carter",
                    DisplayName = "John",
                    ProfilePic = "/Avatar/myProfilePic.jpg"
                }, "Blog1234!");
            }

            var userId = userManager.FindByEmail("Jc56wrestling@yahoo.com").Id;
            userManager.AddToRole(userId, "Admin");

            //ProjectManager

            if (!context.Users.Any(u => u.Email == "ProjectManager@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "ProjectManager@Mailinator.com",
                    UserName = "ProjectManager@Mailinator.com",
                    FirstName = "Project",
                    LastName = "Manager",
                    DisplayName = "Im a PM"
                }, "Blog1234!");
            }

            userId = userManager.FindByEmail("ProjectManager@Mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            //SeededManager

            if (!context.Users.Any(u => u.Email == "SeededDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeededDeveloper@Mailinator.com",
                    UserName = "SeededDeveloper@Mailinator.com",
                    FirstName = "Seed",
                    LastName = "Developer",
                    DisplayName = "Im a Developer"
                }, "Blog1234!");
            }

            userId = userManager.FindByEmail("SeededDeveloper@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            //seededSubmitter

            if (!context.Users.Any(u => u.Email == "SeededSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "SeededSubmitter@Mailinator.com",
                    UserName = "SeededSubmitter@Mailinator.com",
                    FirstName = "Seeded",
                    LastName = "Submitter",
                    DisplayName = "Im A Submitter"
                }, "Blog1234!");
            }

            userId = userManager.FindByEmail("SeededSubmitter@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "Immediate", Description = "The highest possible priority" },
                    new TicketPriority { Name = "High", Description = "The 2nd highest possible priority" },
                    new TicketPriority { Name = "Medium", Description = "The mid-level priority" },
                    new TicketPriority { Name = "Low", Description = "The 2nd lowest possible priority" },
                    new TicketPriority { Name = "None", Description = "The lowest possible priority" }

                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Name = "Defect", Description = "A reported defect in a supported project or application" },
                    new TicketType { Name = "New functionality request", Description = "A new request for functionality in a supported project or application" },
                    new TicketType { Name = "Call for documentation", Description = "A reported defect in a supported project or application" }

                );

            context.TicketStatuses.AddOrUpdate(
               t => t.Name,
                   new TicketStatus { Name = "New/Unassigned", Description = "This will be the status of all newly created tickets" },
                   new TicketStatus { Name = "Unassigned", Description = "This will be the status of all newly created tickets" },
                   new TicketStatus { Name = "Assigned", Description = "This will be the status of all tickets assigned to a developer" },
                   new TicketStatus { Name = "Completed", Description = "This will be the status of all completed but unarchived tickets" },
                   new TicketStatus { Name = "Archived", Description = "This will be the status of all completed and archived tickets" },
                   new TicketStatus { Name = "Unknown", Description = "This will be the status of all tickets unknown" }

               );

            //Demouser Section
            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "DemoAdmin@Mailinator.com",
                    UserName = "DemoAdmin@Mailinator.com",
                    FirstName = "Arnold",
                    LastName = "Admin",
                    DisplayName = "Arnold. A"
                }, "Blog1234!");
            }
            userId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            //PM
            if (!context.Users.Any(u => u.Email == "DemoProjectManager@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "DemoProjectManager@Mailinator.com",
                    UserName = "DemoProjectManager@Mailinator.com",
                    FirstName = "Pat",
                    LastName = "ProjectManager",
                    DisplayName = "Pat. M"
                }, "Blog1234!");
            }
            userId = userManager.FindByEmail("DemoProjectManager@Mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");
            
            //dev
            if (!context.Users.Any(u => u.Email == "DemoDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "DemoDeveloper@Mailinator.com",
                    UserName = "DemoDeveloper@Mailinator.com",
                    FirstName = "Devin",
                    LastName = "Developer",
                    DisplayName = "Devin .D"
                }, "Blog1234!");
            }
            userId = userManager.FindByEmail("DemoDeveloper@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            //sub
            if (!context.Users.Any(u => u.Email == "DemoSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "DemoSubmitter@Mailinator.com",
                    UserName = "DemoSubmitter@Mailinator.com",
                    FirstName = "Susie",
                    LastName = "Submitter",
                    DisplayName = "Susie .S"
                }, "Blog1234!");
            }
            userId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

        }
    }
}
