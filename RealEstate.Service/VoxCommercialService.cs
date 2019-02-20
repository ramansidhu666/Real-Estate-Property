using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Property.Entity;
using Dapper;
namespace Property.Service
{
    public interface IVoxCommercialService
    {
        // Main Functions
        List<VoxCommercial> GetVoxCommercials();
        VoxCommercial GetVoxCommercial(string id);
        VoxCommercial InsertVoxCommercial(VoxCommercial VoxCommercial);
        VoxCommercial UpdateVoxCommercial(VoxCommercial VoxCommercial);
        int DeleteVoxCommercial(string id);
        // Other Functions
        VoxCommercial GetVoxCommercialByUserId(int userId);
    }
    public class VoxCommercialService : BaseRepository, IVoxCommercialService
    {
        ErrorLogging errLog;
        public VoxCommercialService()
        {
            //errLog = new ErrorLogging();
        }
        public List<VoxCommercial> GetVoxCommercials()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    string query = "SELECT * FROM PropertyData_Comm_VOX";
                    List<VoxCommercial> VoxCommercialList = _db.Query<VoxCommercial>(query).ToList();
                    return VoxCommercialList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public VoxCommercial GetVoxCommercial(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Comm_VOX  WHERE VoxCommercialId = @VoxCommercialId";
                return _db.Query<VoxCommercial>(query, new { VoxCommercialId = id }).SingleOrDefault();
            }
        }
        public VoxCommercial InsertVoxCommercial(VoxCommercial VoxCommercial)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                    //VoxCommercial.VoxCommercialId = _db.Query<Guid>("InsertUpdateVoxCommercial", VoxCommercial, commandType: CommandType.StoredProcedure).Single();
                    return VoxCommercial;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                    //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public VoxCommercial UpdateVoxCommercial(VoxCommercial VoxCommercial)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                   // VoxCommercial.VoxCommercialId =_db.Query<Guid>("InsertUpdateVoxCommercial", VoxCommercial, commandType: CommandType.StoredProcedure).Single();
                    return VoxCommercial;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                   //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public int DeleteVoxCommercial(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData_Comm_VOX] WHERE [VoxCommercialId]=@VoxCommercialId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { VoxCommercialId = id }, transaction);
                    transaction.Commit();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    string ErrorMsg = ex.Message.ToString();
                    //errLog.LogError(ex);

                    return 0;
                }
            }
        }
        public VoxCommercial GetVoxCommercialByUserId(int UserId)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Comm_VOX  WHERE UserId = @UserId";
                return _db.Query<VoxCommercial>(query, new { UserId = UserId }).SingleOrDefault();
            }
        }
    }

}

