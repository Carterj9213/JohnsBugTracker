using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JohnsBugTracker.Models;
using JohnsBugTracker.ViewModels;

namespace JohnsBugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMorrisBarChartData()
        {
            var ticketTypes = db.TicketTypes.ToList();
            var charData = new List<MorrisBarChartData>();

            foreach (var type in ticketTypes)
            {
                charData.Add(new MorrisBarChartData()
                {
                    TicketType = type.Name,
                    Count = db.Tickets.Where(t => t.TicketType.Name == type.Name).Count()
                });

            }
            return Json(charData);

        }
        //donut chart
        public JsonResult GetMorrisDonutChartData()
        {
            var ticketTypes = db.TicketTypes.ToList();
            var charData = new List<MorrisDonutChartData>();

            foreach (var type in ticketTypes)
            {
                charData.Add(new MorrisDonutChartData()
                {
                    label = type.Name,
                    value = db.Tickets.Where(t => t.TicketType.Name == type.Name).Count()
                });

            }
            return Json(charData);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult LandingPage()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult DemoLogin()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            var model = new EmailModel();
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var standardBodyMessage = $"<h3>You have received an email from {model.FromEmail}</h3>";
                    standardBodyMessage += $"<br /> {model.Body}";

                    var from = "JohnBlog<jc56wrestling@yahoo.com>";
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = $"{standardBodyMessage}",
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return RedirectToAction("Index", "BlogPosts");
                    //return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);

        }


    }
}