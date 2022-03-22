using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using parity_mvc_intro.Models;

namespace parity_mvc_intro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["ActiveUserId"] != null)
            {
                Student student = Student.GetStudent((int)Session["ActiveUserId"]);
                return View(student);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(Models.Student student)
        {
            var log = Student.LogIn(student.Email, student.Password);

            // Success
            if (log.success)
            {
                Session["ActiveUserId"] = log.id;

                TempData["login_success"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["login_success"] = false;
                return RedirectToAction("Index");
            }

        }
    }
}