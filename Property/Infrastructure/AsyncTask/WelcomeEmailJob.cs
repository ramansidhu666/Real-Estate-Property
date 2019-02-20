using Property.Core.Infrastructure;
using Property.Entity;
using Property.Infrastructure.Email;
using Property.Service;
using Quartz;
using System;
using System.Linq;

namespace Property.Web.Infrastructure.AsyncTask
{
    public class WelcomeEmailJob : IJob
    {
        public IUserService _userService { get; set; }
        public IEmailVerificationService _emailVerificationService { get; set; }
        public WelcomeEmailJob(IUserService userService, IEmailVerificationService emailVerificationService)
        {
            this._userService = userService;    
            this._emailVerificationService = emailVerificationService;
        }

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string userId = dataMap.GetString("UserId");
            string emailType = dataMap.GetString("EmailType");
            var user = _userService.GetUser(userId);
            if (user != null)
            {
                //Start : Insert Email Verification
                EmailVerification emailverification = new EmailVerification();
                emailverification.UserId = user.UserId;
                emailverification.EmailType = emailType;
                emailverification.ActiveForMinute = ConstantModel.ProjectSettings.EmailExpireLimit; //Valid upto 24-Hours
                emailverification.IsActive = true;
                emailverification.IsOperationDone = false;
                var emailVerificationResult = _emailVerificationService.InsertEmailVerification(emailverification);
                //End : Insert Email Verification
                    
                //Start : Sending welcome mail after registeration or Reverify account email
                EmailManager emailManager = new EmailManager();
                EMailEntity emailEntity = new EMailEntity();
                emailEntity.ToMail = user.EmailId;
                emailEntity.RequestId = emailVerificationResult.EmailVerificationId.ToString(); //Here RequestId is EmailVerificationId
                emailEntity.FirstName = user.FirstName;
                if (emailType == EnumValue.GetEnumDescription(EnumValue.EmailType.WelcomeEmail))
                {
                    emailManager.SendMailOfWelcome(emailEntity);
                }
                else if (emailType == EnumValue.GetEnumDescription(EnumValue.EmailType.ReverifyAccount))
                {
                    emailManager.SendMailForReverifyAccount(emailEntity);
                }
                //End : Sending welcome mail after registeration or Reverify account email
            }
        }
    }
}