using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Property.Entity;
using Property.Service;
using Property.Infrastructure;
using Property.Core.UtilityManager;
using Newtonsoft.Json;

namespace Property.Controllers
{

    public class HomeController : Controller
    {
        private IIdxCommercialService _CommercialService { get; set; }
        private IIdxResidentialService _ResidentialService { get; set; }
        private IIdxCondoService _CondoService { get; set; }
        public IGenrateLatLongFromAddressServices _GenrateLatLongFromAddressServices { get; set; }
        public HomeController(IIdxCommercialService CommercialService, IIdxResidentialService ResidentialService, IIdxCondoService CondoService, IGenrateLatLongFromAddressServices GenrateLatLongFromAddressServices)
        {
            this._CommercialService = CommercialService;
            this._ResidentialService = ResidentialService;
            this._CondoService = CondoService;
            this._GenrateLatLongFromAddressServices = GenrateLatLongFromAddressServices;
        }

        // GET: Home
        public ActionResult Index()
        {
            MainModel model = new MainModel();
          
            List<PropertyModel> PropertList = new List<PropertyModel>();        
            PropertList = _ResidentialService.GetResidentials();
            //UpdatePropetyLatLong(PropertList);
            model.PropertiesModel = PropertList.Take(50).ToList();
            model.FeaturedPropertiesModel = PropertList.Take(15).ToList();
            return View(model);
        }

       

        // GET: /get all listing according to property type id/
        public ActionResult GetPropertyList(int PropertyType)
        {
            var StaticPropertyType = "Residential";
            try
            {

                List<PropertyModel> PropertList = new List<PropertyModel>();
                if (PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Residential)
                {
                    PropertList = _ResidentialService.GetResidentials();
                }
                else if (PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Commercial)
                {
                    PropertList = _CommercialService.GetCommercials();
                }
                else if (PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Condo)
                {
                    PropertList = _ResidentialService.GetResidentials();
                }
                else
                {
                    PropertList = null;
                }
                return View(PropertList);
            }
            catch (Exception ex)
            {

                throw;
                return View();
            }


        }
        [HttpPost]
        public ActionResult LoadData()
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var datalist = _ResidentialService.GetIdxResidentials();

            datalist = datalist.OrderBy(x => x.MLS).ToList();

            recordsTotal = datalist.Count();
            var data = datalist.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAddressList()
        {
            List<PropertyModel> PropertList = new List<PropertyModel>();
            var LatLongList = _GenrateLatLongFromAddressServices.GetPropertiesLatLongs().Select(c=>c.MLSID).ToList();
            PropertList = _CommercialService.GetCommercials().Where(c => !LatLongList.Contains(c.MLS.ToString())).ToList();
            var list = JsonConvert.SerializeObject(PropertList,
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
            return Content(list, "application/json");
        }
        public JsonResult SaveLatLong(List<PropertiesLatLongs> properties)
        {
            if (properties != null)
            {
                var LatLongList = _GenrateLatLongFromAddressServices.GetPropertiesLatLongs().ToList();
                foreach (var item in properties)
                {
                    var IsLatLongFound = LatLongList.Where(c => c.MLSID == item.MLSID).FirstOrDefault();
                    if (IsLatLongFound==null)
                    {
                        _GenrateLatLongFromAddressServices.InsertPropertiesLatLongs(item);
                    }
                   
                }
               
                return Json("success");
            }
            else
            {
                return Json("error");
            }
           
        }
        public void UpdatePropetyLatLong(List<PropertyModel> PropertiesList)
        {
            try
            {
                PropertiesLatLongs model = new PropertiesLatLongs();
                var MlsIds = _GenrateLatLongFromAddressServices.GetPropertiesLatLongs().Select(c => c.MLSID).ToList();
                var Properties = PropertiesList.Where(c => !MlsIds.Contains(c.MLS));
                int CheckCount = 1;
                foreach (var property in Properties)
                {

                    if (property.Address != null && property.Address != "")
                    {
                       
                            property.Address += "," + property.Community + "," + property.Municipality;
                            var latlong = RealEstate.Core.UtilityManager.GoogleOperation.GetLatLong(property.Address);
                            if (latlong != null)
                            {
                                //var splitstr = latlong.Split(',');
                                model.longitude = Convert.ToDecimal(latlong[0]);
                                model.latitude = Convert.ToDecimal(latlong[1]);
                                model.MLSID = property.MLS;
                                model.Address = property.Address;
                                _GenrateLatLongFromAddressServices.InsertPropertiesLatLongs(model);
                            }
                            CheckCount = CheckCount + 1;
                        

                        return ;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}