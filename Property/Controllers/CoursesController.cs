using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JEE_course()
        {
            return View();
        }

        public ActionResult NEET_course()
        {
            return View();
        }
        public ActionResult course_detail()
        {
            return View();
        }
    }
}