using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace Property.Web.Infrastructure.AsyncTask
{
    public class JobScheduler
    {
        public static void SendSMS(string MobileNo, string Message)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail SMSJobs = JobBuilder.Create<SMSJob>().UsingJobData("MobileNo", MobileNo).UsingJobData("Message", Message).Build();

            ITrigger SMSTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(30)) //It will send with in 30 seconds
                .Build();

            scheduler.ScheduleJob(SMSJobs, SMSTrigger);
        }

        public static void SendEmail(string UserName, string LastName, string MobileNo, string EmailVerifyCode)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail EmailJobs = JobBuilder.Create<EmailJob>().UsingJobData("UserName", UserName).UsingJobData("LastName", LastName).UsingJobData("MobileNo", MobileNo).UsingJobData("EmailVerifyCode", EmailVerifyCode).Build();

            ITrigger EmailTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(30)) //It will send with in 30 seconds
                .Build();

            scheduler.ScheduleJob(EmailJobs, EmailTrigger);
        }
        public static void WelcomeEmailJob(string userId,string emailType)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail welcomeEmailJobs = JobBuilder.Create<WelcomeEmailJob>().UsingJobData("UserId", userId).UsingJobData("EmailType", emailType).Build();

            ITrigger welcomeEmailTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(15)) //It will send with in 15 seconds
                .Build();

            scheduler.ScheduleJob(welcomeEmailJobs, welcomeEmailTrigger);
        }
    }
}