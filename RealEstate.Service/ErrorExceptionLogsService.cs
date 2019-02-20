
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RealEstate.Entity;
namespace RealEstate.Service
{
    public interface IErrorExceptionLogsService
    {
        ErrorExceptionLogs Find(int? id);
        ErrorExceptionLogs Add(ErrorExceptionLogs ErrorExceptionLogs);
    }
    public class ErrorExceptionLogsService : BaseRepository, IErrorExceptionLogsService
    {
        ErrorLogging errLog;
        public ErrorExceptionLogsService()
        {
            errLog = new ErrorLogging();
        }


        public ErrorExceptionLogs Find(int? id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM ErrorExceptionLogs WHERE EventId = " + id;
                return _db.Query<ErrorExceptionLogs>(query, new { id }).SingleOrDefault();
            }
        }

        public ErrorExceptionLogs Add(ErrorExceptionLogs ErrorExceptionLogs)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Source", ErrorExceptionLogs.Source);
                    parameters.Add("LogDateTime", DateTime.Now.ToString("g"));
                    parameters.Add("Message", ErrorExceptionLogs.Message);
                    parameters.Add("QueryString", ErrorExceptionLogs.QueryString);
                    parameters.Add("TargetSite", ErrorExceptionLogs.TargetSite);
                    parameters.Add("StackTrace", ErrorExceptionLogs.StackTrace);
                    parameters.Add("ServerName", ErrorExceptionLogs.ServerName);
                    parameters.Add("RequestURL", ErrorExceptionLogs.RequestURL);
                    parameters.Add("UserAgent", ErrorExceptionLogs.UserAgent);
                    parameters.Add("UserIP", ErrorExceptionLogs.UserIP);
                    parameters.Add("UserAuthentication", ErrorExceptionLogs.UserAuthentication);
                    parameters.Add("UserName", ErrorExceptionLogs.UserName);
                    parameters.Add("EventId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var EventId = _db.Query<int>("SaveErrorExceptionLogs", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    ErrorExceptionLogs.EventId = EventId;
                    return ErrorExceptionLogs;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                    errLog.LogError(ex);
                    return null;
                }
            }
        }
    }
}
