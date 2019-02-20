using Property.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property.Controllers
{
    public class PartialController : BaseController
    {
        public PartialController(IUserService userService)
            : base(userService)
        {
            this._userService = userService;
        }
        // GET: Header for login
        public ActionResult LoginHeader()
        {
            return View();
        }
        // GET: Footer for login
        public ActionResult LoginFooter()
        {
            return View();
        }
        //GET: Page Not Found
        public ActionResult PageNotFound()
        {
            return View();
        }
        //GET: Page Expire
        public ActionResult PageExpire()
        {
            return View();
        }
    }
}