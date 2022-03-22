using parity_mvc_intro.Models;
using parity_mvc_intro.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace parity_mvc_intro.Controllers
{
    public class CoursesController : Controller
    {
        [RequiresUserValidation]
        public ActionResult Index()
        {
            // Provide a list of available courses to the view
            List<Course> courses = Course.GetAvailableCourses((int)Session["ActiveUserId"]);

            return View(courses);
        }

        [HttpGet]
        [RequiresUserValidation]
        public ActionResult Register(int id, string courseName)
        {
            // Register for course - All logic for registration handled in course class/sql procedure
            if (Course.RegisterCourse((int)Session["ActiveUserId"], id))
            {
                TempData["registration_success"] = courseName;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [RequiresUserValidation]
        public ActionResult Drop(int id, string courseName)
        {
            // Drop course - All logic for registration handled in course class/sql procedure
            if (Course.DropCourse((int)Session["ActiveUserId"], id))
            {
                TempData["drop_success"] = courseName;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}