using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Property.Entity;
using Dapper;
namespace Property.Service
{
    public interface IVoxResidentialService
    {
        // Main Functions
        List<VoxResidential> GetVoxResidentials();
        VoxResidential GetVoxResidential(string id);
        VoxResidential InsertVoxResidential(VoxResidential VoxResidential);
        VoxResidential UpdateVoxResidential(VoxResidential VoxResidential);
        int DeleteVoxResidential(string id);
        // Other Functions
        VoxResidential GetVoxResidentialByUserId(int userId);
    }
    public class VoxResidentialService : BaseRepository, IVoxResidentialService
    {
        ErrorLogging errLog;
        public VoxResidentialService()
        {
            //errLog = new ErrorLogging();
        }
        public List<VoxResidential> GetVoxResidentials()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    string query = "SELECT * FROM PropertyData_Vox_Residential";
                    List<VoxResidential> VoxResidentialList = _db.Query<VoxResidential>(query).ToList();
                    return VoxResidentialList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public VoxResidential GetVoxResidential(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Vox_Residential  WHERE VoxResidentialId = @VoxResidentialId";
                return _db.Query<VoxResidential>(query, new { VoxResidentialId = id }).SingleOrDefault();
            }
        }
        public VoxResidential InsertVoxResidential(VoxResidential VoxResidential)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                    //VoxResidential.VoxResidentialId = _db.Query<Guid>("InsertUpdateVoxResidential", VoxResidential, commandType: CommandType.StoredProcedure).Single();
                    return VoxResidential;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                    //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public VoxResidential UpdateVoxResidential(VoxResidential VoxResidential)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                   // VoxResidential.VoxResidentialId =_db.Query<Guid>("InsertUpdateVoxResidential", VoxResidential, commandType: CommandType.StoredProcedure).Single();
                    return VoxResidential;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                   //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public int DeleteVoxResidential(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData_Vox_Residential] WHERE [VoxResidentialId]=@VoxResidentialId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { VoxResidentialId = id }, transaction);
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
        public VoxResidential GetVoxResidentialByUserId(int UserId)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Vox_Residential  WHERE UserId = @UserId";
                return _db.Query<VoxResidential>(query, new { UserId = UserId }).SingleOrDefault();
            }
        }
    }

}

