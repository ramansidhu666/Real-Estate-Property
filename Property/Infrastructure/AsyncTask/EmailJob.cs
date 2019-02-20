using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Twilio.Mvc;
using Twilio;

using Quartz;
using Property.Infrastructure;
namespace Property.Web.Infrastructure.AsyncTask
{
    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string UserName = dataMap.GetString("UserName");
            string LastName = dataMap.GetString("LastName");
            string EmailVerifyCode = dataMap.GetString("EmailVerifyCode");
            string MobileNo = dataMap.GetString("MobileNo");

            //Send messsege//
            SendMailToUser(UserName, LastName, MobileNo, EmailVerifyCode);
            //End :Send Email to Users
        }

        public string SendMailToUser(string UserName, string LastName, string MobileNo, string EmailVerifyCode)
        {
            string result = "";

            try
            {
                // Send mail.
                MailMessage mail = new MailMessage();

                string FromEmailID = WebConfigurationManager.AppSettings["FromEmailID"];
                string FromEmailPassword = WebConfigurationManager.AppSettings["FromEmailPassword"];
                string ToEmailIDs = WebConfigurationManager.AppSettings["ToEmailID"];

                SmtpClient smtpClient = new SmtpClient(WebConfigurationManager.AppSettings["SmtpServer"]);
                int _Port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"].ToString());
                Boolean _UseDefaultCredentials = Convert.ToBoolean(WebConfigurationManager.AppSettings["UseDefaultCredentials"].ToString());
                Boolean _EnableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["EnableSsl"].ToString());

                foreach (var ToEmailID in ToEmailIDs.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(new MailAddress(ToEmailID));
                }

                mail.From = new MailAddress(FromEmailID);
                mail.Subject = "VerificationCode -Property App";
                //string LogoPath = Common.GetURL() + "/images/logo.png";
                string msgbody = "";
                msgbody += "";
                msgbody += "<div style='margin: 0px; padding: 0px; overflow-y: auto; overflow-x: hidden;'>";
                msgbody += "    <div style='background: #ffffff; width: 100%; height: auto; float: left; margin: 0px; padding: 0px;'>";
                //msgbody += "        <div style='padding: 5px; float: left; text-align: left; margin-bottom: 10px;'><img width='100%' src='" + LogoPath + "' alt='Ghetty' title='Ghetty' /></div>";
                msgbody += "    </div>";
                msgbody += "    <span style='font-size: 15px; color: black; padding: 5px; 10px 0 0 0px; float: left;font-family: Gotham, Helvetica Neue, Helvetica, Arial, sans-serif; width: 20%;'>FirstName:" + " " + UserName + "</span>";
                if (LastName != null && LastName != "")
                {
                    msgbody += "<span style='font-size: 15px; color: black; padding: 5px; margin-top: 30px;margin-left:-20%;float: left; font-family: Gotham, Helvetica Neue, Helvetica, Arial, sans-serif;width: 20%;'>LastName:" + " " + LastName + "</span>";
                }
                msgbody += "    <span style='font-size: 15px; color: black; padding: 5px;margin: 10px 0 0 0px; float: left; font-family: Gotham, Helvetica Neue, Helvetica, Arial, sans-serif;width: 100%;'>Mobile No.:" + " " + MobileNo + " </span>";
                msgbody += "    <span style='font-size: 27px; color: black; padding: 5px; margin: 10px 0 0 0px;float: left; font-family: Gotham, Helvetica Neue, Helvetica, Arial, sans-serif;width: 100%;'>Code:" + " " + EmailVerifyCode + "</span>";
                msgbody += "    <span style='font-size: 15px; color: black; padding: 5px;margin: 10px 0 0 0px; float: left; font-family: Gotham, Helvetica Neue, Helvetica, Arial, sans-serif;width: 100%;'>Regards Property Team. </span>";
                msgbody += "    <br />";
                msgbody += "</div>";

                mail.Body = msgbody;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; //_Host;
                smtp.Port = _Port;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = _UseDefaultCredentials;
                smtp.Credentials = new System.Net.NetworkCredential(FromEmailID, FromEmailPassword);// Enter senders User name and password
                smtp.EnableSsl = _EnableSsl;
                smtp.Send(mail);
                result = "success";

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.InnerException.ToString();
                result = ErrorMsg;
            }
            return result;
        }
    }

}