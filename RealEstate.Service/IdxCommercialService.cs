using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Dapper;
using System.IO;
using System.Web;
using RealEstate.Service;
using RealEstate.Entity;
namespace Property.Service
{
    public interface IIdxCommercialService
    {
        // Main Functions
        List<PropertyModel> GetIdxCommercial();
        List<PropertyModel> GetCommercials();
        PropertyModel GetSingleProperty(string MLS);
        List<PropertyModel> SearchProperty(SearchModel model);
        int DeleteIdxCommercial(string id);

    }
    public class IdxCommercialService : BaseRepository, IIdxCommercialService
    {
        ErrorLogging errLog;
        public IdxCommercialService()
        {
            //errLog = new ErrorLogging();
        }
        public List<PropertyModel> GetCommercials()
        {
            try
            {
                using (IDbConnection _db = OpenConnection())
                {
                    var perameters = new DynamicParameters();
                    perameters.Add("@MLSID_City_PostalCode", '0');
                    perameters.Add("@MinPrice", '0');
                    perameters.Add("@MaxPrice", '0');
                    perameters.Add("@BathRooms", '0');
                    perameters.Add("@PropertyType", '0');
                    perameters.Add("@SaleLease", '0');
                    List<PropertyModel> IdxCommercialList = _db.Query<PropertyModel>("GetPropertyData_Comm", perameters, commandType: CommandType.StoredProcedure).ToList();
                    return IdxCommercialList;
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
                    perameters.Add("@BathRooms", '0');
                    perameters.Add("@PropertyType", '0');
                    perameters.Add("@SaleLease", '0');
                    PropertyModel IdxCommercial = _db.Query<PropertyModel>("GetPropertyData_Comm", perameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    try
                    {
                        string sourcePath;
                        var ImageRootPath = @"C:\MlsData\";//ConfigurationManager.AppSettings["ImageURL"].ToString(); ;
                        if (IdxCommercial.VOX.ToString().ToLower() == "false")
                        {

                            sourcePath = ImageRootPath + "IDXImagesCommercial";                            
                        }
                        else
                        {
                            sourcePath = ImageRootPath + "VoxCommercial";
                        }
                        List<PropertyImages> imagelist = new List<PropertyImages>();
                        DirectoryInfo dir = new DirectoryInfo(sourcePath);
                        if (IdxCommercial != null)
                        {
                            foreach (FileInfo files in dir.GetFiles("Photo" + IdxCommercial.MLS.ToString() + "*.*"))
                            {
                                PropertyImages image = new PropertyImages();
                                image.MLS = IdxCommercial.MLS.ToString();
                                image.Image = IdxCommercial.serverimagepath.ToString() + files.Name;
                                imagelist.Add(image);
                                IdxCommercial.PropertyImages = imagelist;
                            }
                        }
                        return IdxCommercial;

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    
                    
                    return IdxCommercial;
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
                    perameters.Add("@BathRooms", model.BathRooms);
                    perameters.Add("@PropertyType", model.PropertyType);
                    perameters.Add("@SaleLease", model.SaleLease);
                    List<PropertyModel> IdxCommercialList = _db.Query<PropertyModel>("GetPropertyData_Comm", perameters, commandType: CommandType.StoredProcedure).ToList();
                    return IdxCommercialList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PropertyModel> GetIdxCommercial()
        {
            using (IDbConnection _db = OpenConnection())
            {

                string query = "SELECT * FROM PropertyData_Comm";
                List<PropertyModel> IdxCommercialList = _db.Query<PropertyModel>(query).ToList();
                return IdxCommercialList;
            }
        }

        public int DeleteIdxCommercial(string id)
        {
            using (IDbConnection _db = OpenConnection())
            {
                string queryInvitedUserDetail = " DELETE FROM [PropertyData_Comm] WHERE [MLS]=@IdxCommercialId";
                IDbTransaction transaction = _db.BeginTransaction();
                try
                {
                    int rowsAffected = _db.Execute(queryInvitedUserDetail, new { IdxCommercialId = id }, transaction);
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

