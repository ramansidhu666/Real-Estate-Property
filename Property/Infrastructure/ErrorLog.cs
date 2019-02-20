using Property.Entity;
using Property.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Property.Infrastructure
{
    public class ErrorLog
    {
        public IErrorExceptionLogsService _errorExceptionLogsService;
        public ErrorLog()
        {
            _errorExceptionLogsService = new ErrorExceptionLogsService();
        }

        public void LogError(Exception ex)
        {
            ErrorExceptionLogs ErrorExceptionLogs = new Entity.ErrorExceptionLogs();
            HttpContext ctxObject = HttpContext.Current;

            ErrorExceptionLogs.LogDateTime = Convert.ToDateTime(DateTime.Now.ToString("g"));
            ErrorExceptionLogs.RequestURL = (ctxObject.Request.Url != null) ? ctxObject.Request.Url.ToString() : String.Empty;
            ErrorExceptionLogs.QueryString = (ctxObject.Request.QueryString != null) ? ctxObject.Request.QueryString.ToString() : String.Empty;
            ErrorExceptionLogs.ServerName = String.Empty;
            if (ctxObject.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                ErrorExceptionLogs.ServerName = ctxObject.Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            ErrorExceptionLogs.UserAgent = (ctxObject.Request.UserAgent != null) ? ctxObject.Request.UserAgent : String.Empty;
            ErrorExceptionLogs.UserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            ErrorExceptionLogs.UserAuthentication = (ctxObject.User.Identity.IsAuthenticated.ToString() != null) ? ctxObject.User.Identity.IsAuthenticated.ToString() : String.Empty;
            ErrorExceptionLogs.UserName = (ctxObject.User.Identity.Name != null) ? ctxObject.User.Identity.Name : String.Empty;
            while (ex != null)
            {
                ErrorExceptionLogs.Source = ex.Source;
                ErrorExceptionLogs.Message = ex.Message;
                ErrorExceptionLogs.TargetSite = ex.TargetSite.ToString();
                ErrorExceptionLogs.StackTrace = ex.StackTrace;

                ex = ex.InnerException;
            }

            _errorExceptionLogsService.Add(ErrorExceptionLogs);

        }

    }
}
