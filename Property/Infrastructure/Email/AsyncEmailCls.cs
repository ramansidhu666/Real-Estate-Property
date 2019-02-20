using System;
using System.Runtime.Remoting.Messaging;
using System.Web.Mail;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Globalization;
using Property.Service;

namespace Property.Infrastructure.Email
{
    #region Public Methods
    public class AsyncEmailCls
    {

        string username = string.Empty;
        string password = string.Empty;
        #region Commented Code to define the usage of class

        // }
        /// <summary>
        /// USAGE:
        /// EmailUtil eu = new EmailUtil(); 
        /// eu.HtmlBody=@"c:\temp\file.htm";
        /// eu.Subject ="test message";
        /// eu.SmtpServer="mail.myserver.com";
        /// eu.FromEmail="you@yourserver.com";
        /// (loop here through your datatable etc. of email recipients ---)
        /// eu.SendEmailAsync("xxxxxxxxx", "xxxxxxxxx@yahoo.com");

        /// </summary>
        //public class EmailUtil
        //{
        #endregion

        #region Variables and Properties



        private string htmlBody;
        public string HtmlBody
        {
            get
            {
                return htmlBody;
            }
            set
            {
                htmlBody = value;
            }
        }
        private string subject;
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        private string smtpServer;
        public string SmtpServer
        {
            get
            {
                return smtpServer;
            }
            set
            {
                smtpServer = value;
            }
        }

        private string fromEmail;
        public string FromEmail
        {
            get
            {
                return fromEmail;
            }

            set
            {
                fromEmail = value;
            }
        }
        private Int32 port;
        public Int32 Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }
        public string strBody = String.Empty;


        #endregion

        #region Methods
        /// <summary>
        /// The below method is used to send mail.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public string SendEmail(string name, string emailAddress)
        {
            if (strBody == String.Empty)
            {
                try
                {
                    strBody = this.htmlBody;
                }
                catch (Exception ex)
                {
                    throw new Exception("error reading HTML File" + ex.Message);
                }
            }
            try
            {
                username = ConfigurationManager.AppSettings["UserName"].ToString();
                password = ConfigurationManager.AppSettings["Password"].ToString();
                System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
                Message.IsBodyHtml = true;
                Message.To.Add(emailAddress);
                Message.From = new MailAddress(this.FromEmail);
                Message.Subject = this.Subject;
                Message.Body = strBody;
                SmtpClient smtp = new SmtpClient();
                SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTP"].ToString();
                smtp.Host = this.smtpServer;

                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                smtp.Credentials = new System.Net.NetworkCredential(username, password);

                //smtp.EnableSsl = true;

                smtp.Send(Message);
            }
            catch //(Exception ehttp)
            {
                //throw new Exception("Send error" + ehttp.Message);
            }

            return "sent" + name;
        }
        public delegate string SendEmailDelegate(string name, string emailAddress);
        /// <summary>
        /// function to get the result callback method
        /// </summary>
        /// <param name="ar"></param>
        public void GetResultsOnCallback(IAsyncResult ar)
        {
            SendEmailDelegate del = (SendEmailDelegate)
             ((AsyncResult)ar).AsyncDelegate;
            try
            {
                string result;
                result = del.EndInvoke(ar);
                Debug.WriteLine("\nOn CallBack: result is " + result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nOn CallBack, problem occurred: " + ex.Message);
            }
        }
        /// <summary>
        /// This function is used to send mail and invoking threads 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public string SendEmailAsync(string name, string emailAddress)
        {
            SendEmailDelegate dc = new SendEmailDelegate(this.SendEmail);
            AsyncCallback cb = new AsyncCallback(this.GetResultsOnCallback);
            IAsyncResult ar = dc.BeginInvoke(name, emailAddress, cb, null);
            return "ok";
        }
        #endregion
    } // end class AsyncEmailCls 

    public static class SendMailWithAttachment
    {
        public static void SendMail(string fromAddress, string toAddress, string subject, string body)
        {
            using (System.Net.Mail.MailMessage mail = (System.Net.Mail.MailMessage)BuildMessageWith(fromAddress, toAddress.Replace(',', ';'), subject, body))
            {
                SendMail(mail);
            }
        }
        public static void SendMail(System.Net.Mail.MailMessage mail)
        {
            try
            {
                Boolean _UseDefaultCredentials = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseDefaultCredentials"].ToString());
                Boolean _EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSsl"].ToString());

                System.Net.NetworkCredential basicAuthenticationInfo1 = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["MailAddress"], System.Configuration.ConfigurationManager.AppSettings["MailPassword"]);
                SmtpClient objSmptp = new SmtpClient();//ConfigurationManager.AppSettings["SMTP"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString()));
                objSmptp.Host = System.Configuration.ConfigurationManager.AppSettings["SMTP"].ToString();
                objSmptp.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                objSmptp.EnableSsl = _EnableSsl;
                objSmptp.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmptp.UseDefaultCredentials = _UseDefaultCredentials;
                objSmptp.Credentials = basicAuthenticationInfo1;
                objSmptp.Send(mail);
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
            }
        }
        //build a mail message
        private static System.Net.Mail.MailMessage BuildMessageWith(string fromAddress, string toAddress, string subject, string body)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                {
                    Sender = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString()), // on Behave of When From differs
                    From = new MailAddress(fromAddress, ConfigurationManager.AppSettings["DisplayName"].ToString()),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                string[] tos = toAddress.Split(';');

                foreach (string to in tos)
                {

                    message.To.Add(new MailAddress(to));
                }

                return message;
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                ErrorLogging errLog = new ErrorLogging();
                errLog.LogError(ex);
                return null;
            }
        }
        // read the text in template file and return it as a string
        private static string ReadFileFrom(string templateName)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/EmailNotifiTemplate/" + templateName);

            string body = File.ReadAllText(filePath);

            return body;
        }
        // get the template body, cache it and return the text
        private static string GetMailBodyOfTemplate(string templateName)
        {
            string cacheKey = string.Concat("mailTemplate:", templateName);
            string body;
            body = (string)System.Web.HttpContext.Current.Cache[cacheKey];
            if (string.IsNullOrEmpty(body))
            {
                //read template file text
                body = ReadFileFrom(templateName);

                if (!string.IsNullOrEmpty(body))
                {
                    System.Web.HttpContext.Current.Cache.Insert(cacheKey, body, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            return body;
        }
        // replace the tokens in template body with corresponding values
        public static string PrepareMailBodyWith(string templateName, params string[] pairs)
        {
            string body = GetMailBodyOfTemplate(templateName);

            for (var i = 0; i < pairs.Length; i += 2)
            {
                body = body.Replace("<%={0}%>".FormatWith(pairs[i]), pairs[i + 1]);
            }
            return body;
        }

    }

    public static class ExtensionsClass
    {
        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, target, args);
        }
    }
    #endregion
}
