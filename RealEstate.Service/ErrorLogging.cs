using System;
using System.Data;
using System.Diagnostics;
using Dapper;

using System.Text;
using RealEstate.Entity;

namespace RealEstate.Service
{
    public class ErrorLogging : BaseRepository
    {
        public void LogError(Exception ex)
        {
            string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;
            while (ex != null)
            {
                strMessage = ex.Message;
                strSource = ex.Source;
               strTargetSite = ex.TargetSite.ToString();
                strStackTrace = ex.StackTrace;
                ex = ex.InnerException;
            }

            if (strMessage.Length > 0)
            {
                try
                {
                    ErrorExceptionLogs errorExceptionLog = new ErrorExceptionLogs();
                    errorExceptionLog.RequestURL = GetExecutingMethodName(ex);

                    var parameters = new DynamicParameters();
                    parameters.Add("Source", strSource);
                    parameters.Add("LogDateTime", DateTime.Now.ToString("g"));
                    parameters.Add("Message", strMessage);
                    parameters.Add("QueryString", errorExceptionLog.QueryString);
                    parameters.Add("TargetSite", strTargetSite);
                    parameters.Add("StackTrace", strStackTrace);
                    parameters.Add("ServerName", errorExceptionLog.ServerName);
                    parameters.Add("RequestURL", errorExceptionLog.RequestURL);
                    parameters.Add("UserAgent", errorExceptionLog.UserAgent);
                    parameters.Add("UserIP", errorExceptionLog.UserIP);
                    parameters.Add("UserAuthentication", errorExceptionLog.UserAuthentication);
                    parameters.Add("UserName", errorExceptionLog.UserName);
                    parameters.Add("EventId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    using (IDbConnection _db = OpenConnection())
                    {
                        string query = "SaveErrorExceptionLogs";
                        _db.Execute(query, parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception exc)
                {
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("Database Error From Exception Log!");
                    //sb.Append("Source : " + exc.Source);
                    //sb.Append("Message : " + exc.Message);
                    //WriteLog(sb.ToString());
                    EventLog.WriteEntry(exc.Source, "Database Error From Exception Log!", EventLogEntryType.Error, 65535);
                }
            }
        }
        private static string GetExecutingMethodName(Exception exception)
        {
            var trace = new StackTrace(exception);
            var frame = trace.GetFrame(0);
            var method = frame.GetMethod();

            return string.Concat(method.DeclaringType.FullName, ".", method.Name);
        }
        public static void WriteLog(string Message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("==============================================================================" + Environment.NewLine);
            sb.Append("Error occurred on : " + DateTime.Now + Environment.NewLine);
            sb.Append(Message + Environment.NewLine);
            sb.Append("==============================================================================" + Environment.NewLine);

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\ErrorLog.txt";

            System.IO.File.AppendAllText(path.Replace("file:\\", ""), sb.ToString());
        }
    }
}
