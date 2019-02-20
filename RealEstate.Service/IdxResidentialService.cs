using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using RealEstate.Entity;
using Dapper;
using System.IO;
using System.Web;
using RealEstate.Service;
namespace RealEstate.Service
{
    public interface IIdxResidentialService
    {
        // Main Functions
        List<PropertyModel> GetIdxResidentials();
        List<PropertyModel> GetResidentials();
        PropertyModel GetSingleProperty(string MLS);
        List<PropertyModel> SearchProperty(SearchModel model);
        int DeleteIdxResidential(string id);

    }
    public class IdxResidentialService : BaseRepository, IIdxResidentialService
    {
        ErrorLogging errLog;
        public IdxResidentialService()
        {
            //errLog = new ErrorLogging();
        }
        public List<PropertyModel> GetResidentials()
        {
            List<PropertyModel> IdxResidentialList = null;
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
                    IdxResidentialList = _db.Query<PropertyModel>("GetResidentialProperties", perameters, commandType: CommandType.StoredProcedure).ToList();
                    
                    return IdxResidentialList;
                }
            }
            catch (Exception ex)
            {
                return IdxResidentialList;
            }

        }
        public List<PropertyModel> GetIdxResidentials()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    string query = "SELECT * FROM PropertyData";
                    List<PropertyModel> IdxResidentialList = _db.Query<PropertyModel>(query).ToList();
                    return IdxResidentialList;
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
                    PropertyModel IdxResidential = _db.Query<PropertyModel>("GetProperties_Condo", perameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    try
                    {
                        string sourcePath;
                        var ImageRootPath = @"C:\MlsData\";//ConfigurationManager.AppSettings["ImageURL"].ToString(); ;
                        if (IdxResidential.VOX.ToString().ToLower() == "false")
                        {
                            sourcePath = ImageRootPath + "IDXResidential";                            
                        }
                        else
                        {
                            sourcePath = ImageRootPath + "VoxResidential";
                        }
                        List<PropertyImages> imagelist = new List<PropertyImages>();
                        DirectoryInfo dir = new DirectoryInfo(sourcePath);
                        if (IdxResidential != null)
                        {
                            foreach (FileInfo files in dir.GetFiles("Photo" + IdxResidential.MLS.ToString() + "*.*"))
                            {
                                PropertyImages image = new PropertyImages();
                                image.MLS = IdxResidential.MLS.ToString();
                                image.Image = IdxResidential.serverimagepath.ToString() + files.Name;
                                imagelist.Add(image);
                                IdxResidential.PropertyImages = imagelist;
                            }
                        }
                        return IdxResidential;

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                  
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
                    List<PropertyModel> IdxResidentialList = _db.Query<PropertyModel>("GetProperties_Condo", perameters, commandType: CommandType.StoredProcedure).ToList();
                    return IdxResidentialList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteIdxResidential(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData] WHERE [MLS]=@IdxResidentialId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { IdxResidentialId = id }, transaction);
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

