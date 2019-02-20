using Property.Entity;
using Property.Models;
using Property.Service;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.Hosting;

namespace Property.Infrastructure.Email
{
    public class EmailManager : IDisposable
    {
        #region Private Methods
        readonly string _SmtpServer = string.Empty;
        readonly string _FromMail = string.Empty;
        readonly int _SMTPPort = 25;
        readonly string _WebApplicationUrl = string.Empty;
        readonly bool IsMailEnabled = false;

        #endregion

        #region Public Methods
        public EmailManager()
            : base()
        {
            _WebApplicationUrl = CommonClass.GetURL();
            _SmtpServer = ConfigurationManager.AppSettings["SMTP"].ToString();
            _FromMail = ConfigurationManager.AppSettings["MailFrom"].ToString();
            int.TryParse(ConfigurationManager.AppSettings["SMTPPort"].ToString(), out _SMTPPort);
            bool.TryParse(ConfigurationManager.AppSettings["IsMailEnabled"].ToString(), out IsMailEnabled);
        }

        public int SendMailForResetPassword(EMailEntity emailEntity)
        {
            MailMessage mail = new MailMessage();
            MailAddress Sender = new MailAddress(ConfigurationManager.AppSettings["MailFrom"]);
            string RequestPath = CommonClass.GetURL() + "/#/resetpassword?rel_id";
            emailEntity.FromMail = _FromMail;
            mail.To.Add(emailEntity.ToMail);
            mail.From = Sender;
            mail.Subject = "Forgot Password";   
            string body = string.Empty;
            string Logopath = CommonClass.GetURL() + "/Content/verification_email/aws-marketplace-logo.png";
      
            try
            {
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/verification_email/ResetPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{UserName}", emailEntity.FirstName);
                body = body.Replace("{RequestId}", emailEntity.RequestId);
                body = body.Replace("{RequestPath}", RequestPath);
                body = body.Replace("{LogoPath}", Logopath);
                body = body.Replace("{Display_Name}", ConstantModel.ProjectSettings.ProjectDisplayName);
                body = body.Replace("{Tag_Line}", ConstantModel.ProjectSettings.TagLine);
                body = body.Replace("{Owner_Name}", ConstantModel.ProjectSettings.OwnerName);
                body = body.Replace("{Footer_Display_Name}", ConstantModel.ProjectSettings.FooterDisplayName);
                body = body.Replace("{Footer_Display_Address}", ConstantModel.ProjectSettings.FooterDisplayAddress);
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mail.AlternateViews.Add(avHtml);
                mail.IsBodyHtml = true;
                SendMailWithAttachment.SendMail(emailEntity.FromMail, emailEntity.ToMail, "Reset Password", body.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
                return 0;
            }
        }

        public int SendMailForConfirmResetPassword(EMailEntity emailEntity)
        {
            MailMessage mail = new MailMessage();
            MailAddress Sender = new MailAddress(ConfigurationManager.AppSettings["MailFrom"]);
            emailEntity.FromMail = _FromMail;
            mail.To.Add(emailEntity.ToMail);
            mail.From = Sender;
            mail.Subject = "Confirmation Of Password Change";
            string body = string.Empty;
            string Logopath = CommonClass.GetURL() + "/Content/verification_email/aws-marketplace-logo.png";
            string Contactus = CommonClass.GetURL() + "/#/contactus";
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/verification_email/ConfirmResetPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", emailEntity.FirstName);
            body = body.Replace("{RequestId}", emailEntity.RequestId);
            body = body.Replace("{LogoPath}", Logopath);
            body = body.Replace("{ContactUs}", Contactus);
            body = body.Replace("{Display_Name}", ConstantModel.ProjectSettings.ProjectDisplayName);
            body = body.Replace("{Tag_Line}", ConstantModel.ProjectSettings.TagLine);
            body = body.Replace("{Owner_Name}", ConstantModel.ProjectSettings.OwnerName);
            body = body.Replace("{Footer_Display_Name}", ConstantModel.ProjectSettings.FooterDisplayName);
            body = body.Replace("{Footer_Display_Address}", ConstantModel.ProjectSettings.FooterDisplayAddress);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            mail.AlternateViews.Add(avHtml);
            mail.IsBodyHtml = true;
            try
            {
                SendMailWithAttachment.SendMail(emailEntity.FromMail, emailEntity.ToMail, "Password Changed", body.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
                return 0;
            }
        }
        
        public int SendMailOfWelcome(EMailEntity emailEntity)
        {
            MailMessage mail = new MailMessage();
            MailAddress Sender = new MailAddress(ConfigurationManager.AppSettings["MailFrom"]);
            string WelcomePath = CommonClass.GetURL() + "/#/VerifyAccount?rel_id";
            emailEntity.FromMail = _FromMail;
            mail.To.Add(emailEntity.ToMail);
            mail.From = Sender;
            mail.Subject = "Welcome to " + ConstantModel.ProjectSettings.ProjectDisplayName;
            string body = string.Empty;
            string SiteURL = CommonClass.GetURL();
            string Logopath = SiteURL + "/Content/verification_email/aws-marketplace-logo.png";
            string IconPath1 = SiteURL + "/Content/verification_email/img-1.jpg";
            string IconPath2 = SiteURL + "/Content/verification_email/img-2.jpg";
            string IconPath3 = SiteURL + "/Content/verification_email/img-3.jpg";
            try
            {
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/verification_email/WelcomeEmail.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{UserName}", emailEntity.FirstName);
                body = body.Replace("{RequestId}", emailEntity.RequestId);
                body = body.Replace("{LogoPath}", Logopath);
                body = body.Replace("{IconPath1}", IconPath1);
                body = body.Replace("{IconPath2}", IconPath2);
                body = body.Replace("{IconPath3}", IconPath3);
                body = body.Replace("{Welcome}", WelcomePath);
                body = body.Replace("{SiteURL}", SiteURL);

                body = body.Replace("{Display_Name}", ConstantModel.ProjectSettings.ProjectDisplayName);
                body = body.Replace("{Tag_Line}", ConstantModel.ProjectSettings.TagLine);
                body = body.Replace("{Owner_Name}", ConstantModel.ProjectSettings.OwnerName);
                body = body.Replace("{Footer_Display_Name}", ConstantModel.ProjectSettings.FooterDisplayName);
                body = body.Replace("{Footer_Display_Address}", ConstantModel.ProjectSettings.FooterDisplayAddress);
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mail.AlternateViews.Add(avHtml);
                mail.IsBodyHtml = true;
                SendMailWithAttachment.SendMail(emailEntity.FromMail, emailEntity.ToMail, "Welcome to " + ConstantModel.ProjectSettings.ProjectDisplayName, body.ToString());
                return 1;

            }

            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
                return 0;
            }
        }
        public int SendMailForReverifyAccount(EMailEntity emailEntity)
        {
            MailMessage mail = new MailMessage();
            MailAddress Sender = new MailAddress(ConfigurationManager.AppSettings["MailFrom"]);
            string WelcomePath = CommonClass.GetURL() + "/#/VerifyAccount?rel_id";
            emailEntity.FromMail = _FromMail;
            mail.To.Add(emailEntity.ToMail);
            mail.From = Sender;
            mail.Subject = "Verify your account for " + ConstantModel.ProjectSettings.ProjectDisplayName;
            string body = string.Empty;
            string SiteURL = CommonClass.GetURL();
            string Logopath = SiteURL + "/Content/verification_email/aws-marketplace-logo.png";
            try
            {
                using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/verification_email/ReverifyAccount.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{UserName}", emailEntity.FirstName);
                body = body.Replace("{RequestId}", emailEntity.RequestId);
                body = body.Replace("{LogoPath}", Logopath);
                body = body.Replace("{Welcome}", WelcomePath);
                body = body.Replace("{SiteURL}", SiteURL);

                body = body.Replace("{Display_Name}", ConstantModel.ProjectSettings.ProjectDisplayName);
                body = body.Replace("{Tag_Line}", ConstantModel.ProjectSettings.TagLine);
                body = body.Replace("{Owner_Name}", ConstantModel.ProjectSettings.OwnerName);
                body = body.Replace("{Footer_Display_Name}", ConstantModel.ProjectSettings.FooterDisplayName);
                body = body.Replace("{Footer_Display_Address}", ConstantModel.ProjectSettings.FooterDisplayAddress);
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mail.AlternateViews.Add(avHtml);
                mail.IsBodyHtml = true;
                SendMailWithAttachment.SendMail(emailEntity.FromMail, emailEntity.ToMail, "Verify your account for " + ConstantModel.ProjectSettings.ProjectDisplayName, body.ToString());
                return 1;

            }

            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
                return 0;
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Dispose();
            }
        }

        #endregion

    }
}

