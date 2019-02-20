using System;
namespace RealEstate.Entity
{
    public class ErrorExceptionLogs
    {
        private int _eventId = 0;
        public int EventId { get { return _eventId; } set { _eventId = value; } }

        private DateTime _logDateTime = new DateTime(1900, 1, 1);
        public DateTime LogDateTime { get { return _logDateTime; } set { _logDateTime = value;  } }

        private string _source = "";
        public string Source { get { return _source; } set { _source = value;  } }

        private string _message = "";
        public string Message { get { return _message; } set { _message = value; } }

        private string _queryString = "";
        public string QueryString { get { return _queryString; } set { _queryString = value; } }

        private string _targetSite = "";
        public string TargetSite { get { return _targetSite; } set { _targetSite = value; } }

        private string _stackTrace = "";
        public string StackTrace { get { return _stackTrace; } set { _stackTrace = value; } }

        private string _serverName = "";
        public string ServerName { get { return _serverName; } set { _serverName = value; } }

        private string _requestURL = "";
        public string RequestURL { get { return _requestURL; } set { _requestURL = value; } }

        private string _userAgent = "";
        public string UserAgent { get { return _userAgent; } set { _userAgent = value; } }

        private string _userIP = "";
        public string UserIP { get { return _userIP; } set { _userIP = value; } }

        private string _userAuthentication = "";
        public string UserAuthentication { get { return _userAuthentication; } set { _userAuthentication = value; } }

        private string _userName = "";
        public string UserName { get { return _userName; } set { _userName = value; } }
    }
}
