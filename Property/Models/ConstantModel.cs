using System;
using System.Configuration;

namespace ConstantModel
{
    public class ProjectSettings
    {
        private static string _projectDisplayName = ConfigurationManager.AppSettings["DisplayName"].ToString();
        public static string ProjectDisplayName { get { return _projectDisplayName; } }

        private static int _superAdminUserId = Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminUserId"].ToString());
        public static int SuperAdminUserId { get { return _superAdminUserId; } }

        private static int _superAdminRoleId = Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminRoleId"].ToString());
        public static int SuperAdminRoleId { get { return _superAdminRoleId; } }

        private static int _defaultCompanyId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCompanyId"].ToString());
        public static int DefaultCompanyId { get { return _defaultCompanyId; } }

        private static int _defaultCountryId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCountryId"].ToString());
        public static int DefaultCountryId { get { return _defaultCountryId; } }

        private static int _defaultStateId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStateId"].ToString());
        public static int DefaultStateId { get { return _defaultStateId; } }

        private static int _defaultCityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"].ToString());
        public static int DefaultCityId { get { return _defaultCityId; } }

        private static int _subscriptionDays = Convert.ToInt32(ConfigurationManager.AppSettings["SubscriptionDays"].ToString());
        public static int SubscriptionDays { get { return _subscriptionDays; } }

        private static string _dateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
        public static string DateFormat { get { return _dateFormat; } }

        private static int _emailExpireLimit = Convert.ToInt32(ConfigurationManager.AppSettings["EmailExpireLimit"].ToString());
        public static int EmailExpireLimit { get { return _emailExpireLimit; } }

        private static string _ownerName = ConfigurationManager.AppSettings["OwnerName"].ToString();
        public static string OwnerName { get { return _ownerName; } }

        private static string _tagLine = ConfigurationManager.AppSettings["TagLine"].ToString();
        public static string TagLine { get { return _tagLine; } }

        private static string _footerDisplayName = ConfigurationManager.AppSettings["FooterDisplayName"].ToString();
        public static string FooterDisplayName { get { return _footerDisplayName; } }

        private static string _footerDisplayAddress = ConfigurationManager.AppSettings["FooterDisplayAddress"].ToString();
        public static string FooterDisplayAddress { get { return _footerDisplayAddress; } }

        private static int _timeZoneInHours = Convert.ToInt32(ConfigurationManager.AppSettings["TimeZoneInHours"].ToString());
        public static int TimeZoneInHours { get { return _timeZoneInHours; } }

        private static int _timeZoneInMin = Convert.ToInt32(ConfigurationManager.AppSettings["TimeZoneInMin"].ToString());
        public static int TimeZoneInMin { get { return _timeZoneInMin; } }

        private static int _serverInHours = Convert.ToInt32(ConfigurationManager.AppSettings["ServerInHours"].ToString());
        public static int ServerInHours { get { return _serverInHours; } }

        private static int _serverInMin = Convert.ToInt32(ConfigurationManager.AppSettings["ServerInMin"].ToString());
        public static int ServerInMin { get { return _serverInMin; } }

        public static int GetTimezoneInMin()
        {
            int timeZoneInMin = (60 * TimeZoneInHours) + TimeZoneInMin;
            return timeZoneInMin;
        }
        public static int GetServerInMin()
        {
            int serverInMin = (60 * ServerInHours) + ServerInMin;
            return serverInMin;
        }
        public static DateTime GetCurrentDate()
        {
            DateTime currenteDate = DateTime.Now.AddMinutes(GetServerInMin());
            return currenteDate;
        }
    }
    public class EmailSettings
    {
        private static string _mailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
        public static string MailFrom { get { return _mailFrom; } }

        private static string _smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
        public static string SMTP { get { return _smtp; } }

        private static string _smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
        public static string SMTPPort { get { return _smtpPort; } }

        private static string _mailAddress = ConfigurationManager.AppSettings["MailAddress"].ToString();
        public static string MailAddress { get { return _mailAddress; } }

        private static string _mailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
        public static string MailPassword { get { return _mailPassword; } }

        private static bool _isMailEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsMailEnabled"].ToString());
        public static bool IsMailEnabled { get { return _isMailEnabled; } }

        private static bool _useDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"].ToString());
        public static bool UseDefaultCredentials { get { return _useDefaultCredentials; } }

        private static bool _enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"].ToString());
        public static bool EnableSSL { get { return _enableSSL; } }
    }
}