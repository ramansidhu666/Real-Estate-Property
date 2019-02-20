using System;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

using Property.Models;
using Property.Service;
using Property.Core.Infrastructure;
using Property.Infrastructure;
using Property.Entity;
using Property.Infrastructure.Email;
using Property.Web.Infrastructure.AsyncTask;

namespace Property.Controllers
{
    public class AccountController : BaseController
    {
        public IEmailVerificationService _emailVerificationService { get; set; }
        public AccountController(IUserService userService, IEmailVerificationService emailVerificationService)
            : base(userService)
        {
            this._userService = userService;
            this._emailVerificationService = emailVerificationService;
        }
        public ActionResult Index()
        {
            //UserPermission("account");
            return View();
        }

        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        public ActionResult Error()
        {
            //UserPermission("account");
            return View();
        }

        // GET: /Account/Forget Password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        // GET: /Account/Reset Password
        public ActionResult ResetPassword()
        {

            return View();
        }

        // GET: /Account/ForgotPasswordMail
        public JsonResult ForgotPasswordMail(RegisterModel registerModel)
        {
            int result = -1;
            try
            {
                var userInfo = base._userService.GetUserByName(registerModel.Email);
                if (userInfo != null)
                {
                    EmailVerification emailverification = new EmailVerification();
                    emailverification.UserId = userInfo.UserId;
                    emailverification.EmailType = EnumValue.GetEnumDescription(EnumValue.EmailType.ResetPassword);
                    emailverification.ActiveForMinute = ConstantModel.ProjectSettings.EmailExpireLimit; //Valid upto 24-Hours
                    emailverification.IsActive = true;
                    emailverification.IsOperationDone = false;
                    var emailVerificationResult = _emailVerificationService.InsertEmailVerification(emailverification);

                    EmailManager emailManager = new EmailManager();
                    EMailEntity emailEntity = new EMailEntity();
                    emailEntity.ToMail = userInfo.EmailId;
                    emailEntity.RequestId = emailVerificationResult.EmailVerificationId.ToString();
                    emailEntity.FirstName = userInfo.FirstName;
                    result = emailManager.SendMailForResetPassword(emailEntity);

                    return Json(Infrastructure.CommonClass.CreateMessage("success", result), JsonRequestBehavior.AllowGet);
                }
                return Json(Infrastructure.CommonClass.CreateMessage("error", "This email id does not exist. Please try again."), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                string ErrorMsg = ex.Message.ToString();
                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Something went wrong.Please try later."), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoginMethod(RegisterModel registerModel)
        {
            var user = _userService.GetUserByName(registerModel.UserName);
            if (user == null)
            {
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Your are not a register user."));
            }
            
            Mapper.CreateMap<RegisterModel, User>();
            User usermaster = Mapper.Map<RegisterModel, User>(registerModel);
            var objuser = base._userService.ValidateUser(usermaster);
            if (objuser != null)
            {
                return Json(Infrastructure.CommonClass.CreateMessage("success", objuser.UserName));

                //Uncomment the code when you want account verification.
                //if (objuser.IsActive)
                //{
                //    //SetSessionVariables(objuser.EmailId); //Here UserName is Email Address
                //    return Json(Infrastructure.CommonClass.CreateMessage("success", objuser.UserName));
                //}
                //else
                //{
                //    return Json(Infrastructure.CommonClass.CreateMessage("error", "Verification of account is pending."));
                //}
            }
            else
            {
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Your username or password do not match. Please try again."));
            }
        }
        public JsonResult Logout()
        {
            System.Web.HttpContext.Current.Response.Cookies.Clear();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return Json(Infrastructure.CommonClass.CreateMessage("success", "Logout successfully."), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Response.Cookies.Clear();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn", "Account");
        }
        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Mapper.CreateMap<RegisterModel, User>();
                    User user = Mapper.Map<RegisterModel, User>(registerModel);
                    user.UserName = registerModel.Email;
                    user.EmailId = registerModel.Email;

                    //Check Email already exist
                    User isEmailFound = _userService.GetUserByName(user.EmailId);
                    if (isEmailFound != null)
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "Email already in use."));
                    }

                    //Insert User
                    user.Password = SecurityFunction.EncryptString(user.Password);
                    user.IsActive = false;
                    var userResult = _userService.InsertUser(user);
                    //End : Insert User 

                    if (userResult != null)
                    {
                        //Start : Add Job for Send Welcome Email
                        JobScheduler.WelcomeEmailJob(user.UserId.ToString(), EnumValue.GetEnumDescription(EnumValue.EmailType.WelcomeEmail));
                        //End : Add Job for Send Welcome Email

                        //return Json(Infrastructure.CommonClass.CreateMessage("success", "Successfully registered. Please check your email for account verification."));
                        return Json(Infrastructure.CommonClass.CreateMessage("success", "Successfully registered. Please check your email."));
                    }
                    else
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "There is problem while saving data."));
                    }

                }
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please fill all the fields"));

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please try again."));
                //var errors = ModelState.Where(x => x.Value.s.SelectMany(key => this.ModelState[key].Errors);
            }
        }
        
        [HttpPost]
        public JsonResult ResetPassword([Bind(Include = "Password,UserId,RequestId")]RegisterModel registerModel)
        {
            try
            {
                Mapper.CreateMap<RegisterModel, User>();
                User userMaster = Mapper.Map<RegisterModel, User>(registerModel);
                User userFound = _userService.GetUser(registerModel.UserId.ToString());
                if (userFound != null)
                {
                    if (!userFound.IsActive)
                    {
                       return Json(Infrastructure.CommonClass.CreateMessage("error", "Your email verification is pending."));
                    }

                    //Update User
                    userFound.Password = SecurityFunction.EncryptString(userMaster.Password);
                    var userDetail = _userService.UpdateUser(userFound);
                    //End : Update User

                    //Start : Update Email Verification 
                    var emailVerification = _emailVerificationService.GetEmailVerification(registerModel.RequestId); //Here RequestId is EmailVerificationId
                    if (emailVerification != null)
                    {
                        emailVerification.IsOperationDone = true;
                        _emailVerificationService.UpdateEmailVerification(emailVerification);
                    }
                    //End : Update Email Verification 

                    //Start : Send Email
                    EmailManager emailManager = new EmailManager();
                    EMailEntity emailEntity = new EMailEntity();
                    emailEntity.ToMail = userFound.EmailId;
                    emailEntity.RequestId = registerModel.RequestId; //There is no use of RequestId. It is just for error handing in SendMailForConfirmResetPassword function.
                    emailEntity.FirstName = userFound.FirstName;
                    emailManager.SendMailForConfirmResetPassword(emailEntity);
                    //End : Send Email
                    return Json(Infrastructure.CommonClass.CreateMessage("success", "Password is changed successfully."));
                }
                else
                {
                    return Json(Infrastructure.CommonClass.CreateMessage("error", "User does not exists."));
               
                }
            }
            catch (Exception ex)
            {
               
                string ErrorMsg = ex.Message.ToString();

                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Please try again."));
            }
        }
        [HttpPost]
        public JsonResult IsLinkExpire(string requestId,string page)
        {
            try
            {
                var emailVerification = _emailVerificationService.GetEmailVerification(requestId); //Here requestid is emailVerificationId
                if (emailVerification == null)
                {
                    return Json(Infrastructure.CommonClass.CreateMessage("error", "RecordNoFound"));
                }
                if (page == "resetpassword")
                {
                    if (emailVerification != null && emailVerification.EmailType != EnumValue.GetEnumDescription(EnumValue.EmailType.ResetPassword))
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "RecordNoFound"));
                    }
                }
                else if (page == "verifyaccount")
                {
                    if (emailVerification != null 
                        && emailVerification.EmailType != EnumValue.GetEnumDescription(EnumValue.EmailType.WelcomeEmail)
                        && emailVerification.EmailType != EnumValue.GetEnumDescription(EnumValue.EmailType.ReverifyAccount))
                    {
                        return Json(Infrastructure.CommonClass.CreateMessage("error", "RecordNoFound"));
                    }
                }
                if (emailVerification.IsActive == false || emailVerification.IsOperationDone == true) //Expire
                {
                    return Json(Infrastructure.CommonClass.CreateMessage("error", "Expired"));
                }
                var expireDateTime = emailVerification.CreatedOn.AddMinutes(emailVerification.ActiveForMinute);
                var serverDateTime = _userService.GetServerDateTime();
                if (expireDateTime <= serverDateTime)
                {
                    emailVerification.IsActive = false; //Deactivate it
                    _emailVerificationService.UpdateEmailVerification(emailVerification);
                    return Json(Infrastructure.CommonClass.CreateMessage("error", "Expired"));
                }

                return Json(Infrastructure.CommonClass.CreateMessage("success", emailVerification.UserId));
            }
            catch (Exception ex)
            {
              
                string ErrorMsg = ex.Message.ToString();

                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "RecordNoFound"));
            }
        }
        public ActionResult EmailLogin()
        {
            string param1 = this.Request.QueryString["rel_id"];
            var objuser1 = base._userService.GetUser(param1);
            User usermaster = new User();
            usermaster.EmailId = objuser1.EmailId;
            usermaster.Password = objuser1.Password;
            var objuser = base._userService.ValidateUser(usermaster);
            if (objuser != null)
            {
                //SetSessionVariables(objuser.Email);
                Session["UserName"] = objuser.UserName;
                Session["UserId"] = objuser.UserId;
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Error", "Account");
                // return Json(Infrastructure.CommonClass.CreateMessage("error", "Your username or password do not match what we have on file. Please try again."));
            }
        }
        public ActionResult ReverifyAccount()
        {
            return View();
        }
        // GET: /Account/ReverifyAccountMail
        public JsonResult ReverifyAccountMail(RegisterModel registerModel)
        {
            int result = -1;
            try
            {
                var userInfo = base._userService.GetUserByName(registerModel.Email);
                if (userInfo != null)
                {
                    if (userInfo.IsActive)
                    {
                        result = 2;
                    }
                    else
                    {
                        //Start : Add Job for Send Welcome Email
                        JobScheduler.WelcomeEmailJob(userInfo.UserId.ToString(), EnumValue.GetEnumDescription(EnumValue.EmailType.ReverifyAccount));
                        result = 1;
                        //End : Add Job for Send Welcome Email
                    }
                    return Json(Infrastructure.CommonClass.CreateMessage("success", result), JsonRequestBehavior.AllowGet);
                }
                return Json(Infrastructure.CommonClass.CreateMessage("error", "This email id does not exist. Please try again."), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                string ErrorMsg = ex.Message.ToString();
                ErrorLog errorlog = new ErrorLog();
                errorlog.LogError(ex);
                return Json(Infrastructure.CommonClass.CreateMessage("error", "Something went wrong.Please try later."), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VerifyAccount()
        {
            return View();
        }
        [HttpPost]
        public JsonResult VerifyAccountData([Bind(Include = "UserId,RequestId")]RegisterModel registerModel)
        {
            try
            {   
                User userFound = _userService.GetUser(registerModel.UserId.ToString());
                if (userFound != null)
                {
                    //Start : Update User
                    //Note : IsActive field of Customer Table will automatically updated in UpdateUser. So there is no need to update the IsActive field of customer here.
                    userFound.IsActive = true;
                    var userDetail = _userService.UpdateUser(userFound);
                    //End : Update User

                    //Start : Update Email Verification 
                    var emailVerification = _emailVerificationService.GetEmailVerification(registerModel.RequestId); //Here RequestId is EmailVerificationId
                    if (emailVerification != null)
                    {
                        emailVerification.IsOperationDone = true;
                        _emailVerificationService.UpdateEmailVerification(emailVerification);
                    }
                    //End : Update Email Verification 

                    return Json(Infrastructure.CommonClass.CreateMessage("success", "Email has been verified successfully."));
                }
                else
                {
                    return Json(Infrastructure.CommonClass.CreateMessage("error", "User does not exists."));

                }
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
