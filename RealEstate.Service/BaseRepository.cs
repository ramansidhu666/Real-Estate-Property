using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using Dapper;

namespace RealEstate.Service
{
    public abstract class BaseRepository
    {
        protected static void SetIdentity<T>(IDbConnection connection, Action<T> setId)
        {
            dynamic identity = connection.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

        protected static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceDataBase"].ConnectionString);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            return connection;
        }
    }
}