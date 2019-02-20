using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property.Controllers
{
    public class WebPartialController : Controller
    {
        // GET: Header for Web
        public ActionResult WebHeader()
        {
            return PartialView();
        }
        // GET: Footer for Web
        public ActionResult WebFooter()
        {
            return PartialView();
        }
    }
}