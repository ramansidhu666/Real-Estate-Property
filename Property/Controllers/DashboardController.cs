using AutoMapper;
using Property.Core.Infrastructure;
using Property.Entity;
using Property.Infrastructure;
using Property.Infrastructure.Email;
using Property.Models;
using Property.Service;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IUserService userService)
            : base(userService)
        {
            this._userService = userService;
        }   
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        // GET: Header for dashboard
        public ActionResult DashboardHeader()
        {
            return View();
        }
        // GET: Header for pages
        public ActionResult PageHeader()
        {
            return View();
        }
       
        // GET: Header for Navigation
        public ActionResult Navigation()
        {
            return View();
        }
        // GET: Get user by ID
        public JsonResult GetUserById()
        {
            string UserId = Session["UserId"].ToString();
            var userDetail = _userService.GetUser(UserId);
            return Json(Infrastructure.CommonClass.CreateMessage("success", userDetail));
        }
       
        // POST: Update user master table
        public JsonResult UpdateUser(RegisterModel registerModel, HttpPostedFileBase file)
        {   
            try
            {           
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<RegisterModel, User>();
                    User userMaster = Mapper.Map<RegisterModel, User>(registerModel);
                    User userFound = _userService.GetUsers().Where(c => c.UserId == userMaster.UserId).FirstOrDefault();
                    if (userFound != null)
                    {
                        userFound.FirstName = userMaster.FirstName;
                        userFound.LastName = userMaster.LastName;
                        userFound.UserName = userMaster.UserName;
                        userFound.EmailId = userMaster.EmailId;
                        var userDetail = _userService.InsertUser(userFound);
                        //End : Insert User 
                        return Json(Infrastructure.CommonClass.CreateMessage("success", userDetail));
                    }
                    else
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "Not updated please try again"));

                    }
                }

                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please fill all the fields."));

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please try again"));
                //var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                //var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);
            }
        }
        [HttpPost]
        public JsonResult ChangePassword([Bind(Include = "OldPassword,Password")] RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string UserId = Session["UserId"].ToString();
                    Mapper.CreateMap<RegisterModel, User>();
                    User usermaster = Mapper.Map<RegisterModel, User>(registerModel);
                    User userDetail = _userService.GetUsers().Where(c => c.UserId.ToString() == UserId).FirstOrDefault();
                    User userFound = _userService.GetUsers().Where(c => c.UserId.ToString() == UserId && c.Password == SecurityFunction.EncryptString(registerModel.OldPassword)).FirstOrDefault();
                    if (userFound != null)
                    {
                        //Change User Password 

                        userFound.Password = SecurityFunction.EncryptString(usermaster.Password);
                        _userService.InsertUser(userFound);
                        //End : Insert User 
                        return Json(Infrastructure.CommonClass.CreateMessage("success", "Successfully updated."));
                    }
                    else
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "Old passsword not correct."));
                    }
                }

                return Json(Infrastructure.CommonClass.CreateMessage("error", "All fields are required"));

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please try again."));
            }
        }
    }
}               