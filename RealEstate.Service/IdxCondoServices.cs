using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Dapper;
using System.IO;
using System.Web;
using RealEstate.Entity;
using RealEstate.Service;


namespace Property.Service
{
    public interface IIdxCondoService
    {
        // Main Functions
        List<PropertyModel> GetIdxCondos();
        List<PropertyModel> GetCondos();
        PropertyModel GetSingleProperty(string MLS);
        List<PropertyModel> SearchProperty(SearchModel model);
        int DeleteIdxCondo(string id);
        
    }
    public class IdxCondoService : BaseRepository, IIdxCondoService
    {
        ErrorLogging errLog;
        public IdxCondoService()
        {
            //errLog = new ErrorLogging();
        }
        public List<PropertyModel> GetIdxCondos()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    string query = "SELECT * FROM PropertyData_Condo";
                    List<PropertyModel> IdxCondoList = _db.Query<PropertyModel>(query).ToList();
                    return IdxCondoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<PropertyModel> GetCondos()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    var perameters = new DynamicParameters();
                    perameters.Add("@MLSID_City_PostalCode", '0');
                    perameters.Add("@MinPrice", '0');
                    perameters.Add("@MaxPrice", '0');
                    perameters.Add("@BedRooms", '0');
                    perameters.Add("@BathRooms", '0');
                    perameters.Add("@PropertyType", '0');
                    perameters.Add("@SaleLease", '0');
                    List<PropertyModel> IdxCondoList = _db.Query<PropertyModel>("GetProperties_Condo", perameters, commandType: CommandType.StoredProcedure).ToList();
                    return IdxCondoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public PropertyModel GetSingleProperty(string MLS)
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    var perameters = new DynamicParameters();
                    perameters.Add("@MLSID_City_PostalCode", MLS);
                    perameters.Add("@MinPrice", '0');
                    perameters.Add("@MaxPrice", '0');
                    perameters.Add("@BedRooms", '0');
                    perameters.Add("@BathRooms", '0');
                    perameters.Add("@PropertyType", '0');
                    perameters.Add("@SaleLease", '0');
                    PropertyModel IdxCondo = _db.Query<PropertyModel>("GetProperties_Condo", perameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    try
                    {
                        string sourcePath;
                        var ImageRootPath = @"C:\MlsData\";//ConfigurationManager.AppSettings["ImageURL"].ToString(); ;
                        if (IdxCondo.VOX.ToString().ToLower() == "false")
                        {
                            sourcePath = ImageRootPath + "IDXImagesCondo";                            
                        }
                        else
                        {
                            sourcePath = ImageRootPath + "VOXCondo";
                        }
                        List<PropertyImages> imagelist = new List<PropertyImages>();
                        DirectoryInfo dir = new DirectoryInfo(sourcePath);
                        if (IdxCondo != null)
                        {
                            foreach (FileInfo files in dir.GetFiles("Photo" + IdxCondo.MLS.ToString() + "*.*"))
                            {
                                PropertyImages image = new PropertyImages();
                                image.MLS = IdxCondo.MLS.ToString();
                                image.Image = IdxCondo.serverimagepath.ToString() + files.Name;
                                imagelist.Add(image);
                                IdxCondo.PropertyImages = imagelist;
                            }
                        }
                        return IdxCondo;

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                    
                    return IdxCondo;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<PropertyModel> SearchProperty(SearchModel model)
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    var perameters = new DynamicParameters();
                    perameters.Add("@MLSID_City_PostalCode", model.MLSID_City_PostalCode);
                    perameters.Add("@MinPrice", model.MinPrice);
                    perameters.Add("@MaxPrice", model.MaxPrice);
                    perameters.Add("@BedRooms", model.BedRooms);
                    perameters.Add("@BathRooms", model.BathRooms);
                    perameters.Add("@PropertyType", model.PropertyType);
                    perameters.Add("@SaleLease", model.SaleLease);
                    List<PropertyModel> IdxCondoList = _db.Query<PropertyModel>("GetProperties_Condo", perameters, commandType: CommandType.StoredProcedure).ToList();
                    return IdxCondoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteIdxCondo(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData_Condo] WHERE [MLS]=@IdxCondoId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { IdxCondoId = id }, transaction);
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
      
    }

}

