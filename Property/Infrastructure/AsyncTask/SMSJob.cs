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
using Property.Service;

namespace Property.Web.Infrastructure.AsyncTask
{
    public class SMSJob : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string MobileNo = dataMap.GetString("MobileNo");
            string Message = dataMap.GetString("Message");

            //Send SMS to Users
            SendSMSToUser(MobileNo, Message);
            //End :Send SMS to Users
        }

        public string SendSMSToUser(string MobileNo, string Message)
        {
            string result = "";
            string url = "";
            var ServiceName = "";
            string success = "";
            var SenderID = "";
            var ApiKey = "";
            try
            {
                if (MobileNo.Substring(0, 3).Contains("+91"))
                {
                    ApiKey = ConfigurationManager.AppSettings["SMSApiKey"].ToString().Trim();
                    SenderID = ConfigurationManager.AppSettings["SMSSenderID"].ToString().Trim();
                    ServiceName = ConfigurationManager.AppSettings["SMSServiceNameLocal"].ToString().Trim();
                    MobileNo = MobileNo.Replace("+", "").Trim();
                    Message = Message.Trim();
                    url = String.Format("http://smsapi.24x7sms.com/api_2.0/SendSMS.aspx?APIKEY={0}&MobileNo={1}&SenderID={2}&Message={3}&ServiceName={4}", ApiKey, MobileNo, SenderID, Message, ServiceName);
                    success = MakeWebRequestGET(url);
                    try
                    {
                        ErrorLogging.WriteLog(success);
                    }
                    catch { }
                    return success;
                }

                else
                {
                    ApiKey = ConfigurationManager.AppSettings["TwilioSid"].ToString().Trim();
                    var AuthToken = ConfigurationManager.AppSettings["TwilioToken"].ToString().Trim();
                    SenderID = ConfigurationManager.AppSettings["TwilioSenderID"].ToString().Trim();
                    var twilio = new TwilioRestClient(ApiKey, AuthToken);
                    Message twiliomessage = twilio.SendMessage(SenderID, MobileNo, Message);
                    try
                    {
                        ErrorLogging.WriteLog("Mobile No :" + MobileNo + ":" + twiliomessage.AccountSid + ";" + twiliomessage.ErrorCode + ";" + twiliomessage.ErrorMessage);
                    }
                    catch { }
                    return twiliomessage.ToString();
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.InnerException.ToString();
                result = ErrorMsg;
            }
            return result;
        }
        public static string MakeWebRequestGET(string url) //url is http api
        {
            string result = "";
            try
            {
                WebRequest WReq = WebRequest.Create(url);
                WebResponse wResp = WReq.GetResponse();
                StreamReader sr = new StreamReader(wResp.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return result; //result gives you message id
        }
    }
}