using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Property.Entity;
using Dapper;
namespace Property.Service
{
    public interface IVoxCondoService
    {
        // Main Functions
        List<VoxCondo> GetVoxCondos();
        VoxCondo GetVoxCondo(string id);
        VoxCondo InsertVoxCondo(VoxCondo VoxCondo);
        VoxCondo UpdateVoxCondo(VoxCondo VoxCondo);
        int DeleteVoxCondo(string id);
        // Other Functions
        VoxCondo GetVoxCondoByUserId(int userId);
    }
    public class VoxCondoService : BaseRepository, IVoxCondoService
    {
        ErrorLogging errLog;
        public VoxCondoService()
        {
            //errLog = new ErrorLogging();
        }
        public List<VoxCondo> GetVoxCondos()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    string query = "SELECT * FROM PropertyData_Condo_Vox";
                    List<VoxCondo> VoxCondoList = _db.Query<VoxCondo>(query).ToList();
                    return VoxCondoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public VoxCondo GetVoxCondo(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Condo_Vox  WHERE VoxCondoId = @VoxCondoId";
                return _db.Query<VoxCondo>(query, new { VoxCondoId = id }).SingleOrDefault();
            }
        }
        public VoxCondo InsertVoxCondo(VoxCondo VoxCondo)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                    //VoxCondo.VoxCondoId = _db.Query<Guid>("InsertUpdateVoxCondo", VoxCondo, commandType: CommandType.StoredProcedure).Single();
                    return VoxCondo;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                    //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public VoxCondo UpdateVoxCondo(VoxCondo VoxCondo)
        {
            using (IDbConnection _db = OpenConnection())
            {
                try
                {
                   // VoxCondo.VoxCondoId =_db.Query<Guid>("InsertUpdateVoxCondo", VoxCondo, commandType: CommandType.StoredProcedure).Single();
                    return VoxCondo;
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.Message.ToString();
                   //errLog.LogError(ex);
                    return null;
                }
            }
        }
        public int DeleteVoxCondo(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData_Condo_Vox] WHERE [VoxCondoId]=@VoxCondoId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { VoxCondoId = id }, transaction);
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
        public VoxCondo GetVoxCondoByUserId(int UserId)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string query = "SELECT * FROM PropertyData_Condo_Vox  WHERE UserId = @UserId";
                return _db.Query<VoxCondo>(query, new { UserId = UserId }).SingleOrDefault();
            }
        }
    }

}

