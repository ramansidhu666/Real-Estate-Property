using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.SessionState;

using Property.Models;
using Property.Core.Infrastructure;
using Property.Service;
using AutoMapper;
using Property.Infrastructure;
using Property.Entity;

namespace Property.Controllers
{
    public class BaseController : Controller
    {
        public IUserService _userService { get; set; }
        
        public BaseController(IUserService userService)
        {
            this._userService = userService;
        }
        public PartialViewResult ShowProgressBar()
        {
            return PartialView("ShowProgressBar");
        }               
    }
}