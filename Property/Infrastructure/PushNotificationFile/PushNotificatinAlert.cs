
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Property.Infrastructure.PushNotificationFile
{
    public static class PushNotificatinAlert
    {
        public static bool SendPushNotification(string ApplicationId, string UserMessage, string Flag, string JsonMessage, Dictionary<string, object> Dictionary, int BadgeCount = 1, bool IsSoundOn = true)
        {
            UserMessage = (IsSoundOn == false ? "" : UserMessage);
            string Sound = (IsSoundOn == false ? "" : "default");
            var payload1 = new NotificationPayload(ApplicationId, UserMessage, BadgeCount, Sound,"1");
            //payload1.AddCustom("flag", Flag);
            //payload1.AddCustom("message2", JsonMessage);
            //payload1.AddCustom("message1", Dictionary);
            payload1.AddCustom("flag", Flag);
            payload1.AddCustomJson("message2", Dictionary);
            payload1.AddCustom("message1", JsonMessage);
            var p = new List<NotificationPayload> { payload1 };
            var FileName = ConfigurationManager.AppSettings["PushFileName"].ToString(); //Pick from Web Config
            var password = ConfigurationManager.AppSettings["PushPassword"].ToString();//Pick From Web Config
            var P12File = System.Web.HttpContext.Current.Server.MapPath("/Infrastructure/PushNotificationFile/" + FileName);
            var push = new PushNotification(true, P12File, password);
            var rejected = push.SendToApple(p);
            var feedback = push.GetFeedBack();

            if (feedback.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            //foreach (var item in rejected)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.ReadLine();
        }
    }
}
