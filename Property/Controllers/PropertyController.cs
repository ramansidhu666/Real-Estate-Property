using Property.Infrastructure;

using Property.Entity;
using Property.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Property.Controllers
{
    public class PropertyController : Controller
    {

        private IIdxCommercialService _CommercialService { get; set; }
        private IIdxResidentialService _ResidentialService { get; set; }
        private IIdxCondoService _CondoService { get; set; }
        public PropertyController(IIdxCommercialService CommercialService, IIdxResidentialService ResidentialService, IIdxCondoService CondoService)
        {
            this._CommercialService = CommercialService;
            this._ResidentialService = ResidentialService;
            this._CondoService = CondoService;
        }

      

        // GET: /get all listing according to property type id/
        public ActionResult GetPropertyList(int PropertyType)
        {
            try
            {
                List<PropertyModel> PropertList = new List<PropertyModel>();
                if (PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Residential)
                {
                    PropertList = _ResidentialService.GetResidentials();
                }
                else if(PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Commercial)
                {
                    PropertList = _CommercialService.GetCommercials();
                }
                else if (PropertyType > 0 && PropertyType == (int)EnumValue.PropertyType.Condo)
                {
                    PropertList = _CondoService.GetCondos();
                }
                else
                {
                    PropertList = null;
                }
                return View(PropertList.ToList().Take(10));
            }
            catch (Exception ex)
            {
                
                throw;
                return View();
            }

            
        }

        // GET: /search listing /
        [HttpPost]
        public ActionResult SearchProperty(SearchModel model)
        {
            try
            {
                ViewBag.Propertype = model.CurrentPropertyType!=""||model.CurrentPropertyType!=null?model.CurrentPropertyType:"Residential";
                MainModel mainmodel = new MainModel();

                mainmodel.CurrentPropertyType = model.CurrentPropertyType;
                if (model != null)
                {
                    List<PropertyModel> PropertList = new List<PropertyModel>();
                    if (model.CurrentPropertyType != "" && model.CurrentPropertyType == EnumValue.PropertyType.Residential.ToString())
                    {
                        PropertList = _ResidentialService.SearchProperty(model);

                    }
                    else if (model.CurrentPropertyType != "" && model.CurrentPropertyType == EnumValue.PropertyType.Commercial.ToString())
                    {
                        PropertList = _CommercialService.SearchProperty(model);
                    }
                    else if (model.CurrentPropertyType != "" && model.CurrentPropertyType == EnumValue.PropertyType.Condo.ToString())
                    {
                        PropertList = _CondoService.SearchProperty(model);
                    }
                    else
                    {
                        PropertList = null;
                    }
                   
                    mainmodel.FeaturedPropertiesModel = PropertList.Take(3).ToList();
                    var pager = new Pager(PropertList.Count(), 1);
                    mainmodel.Pager = pager;
                    mainmodel.PropertiesModel = PropertList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

                   
                    return View("~/Views/Property/PropertyList.cshtml", mainmodel);
                }

                return View();
            }
            catch (Exception ex)
            {

                throw;
                return View();
            }


        }


        public JsonResult PagingAction(int? page,string CurrentPropertyType)
        {
            try
            {
                List<PropertyModel> PropertList = new List<PropertyModel>();

                if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Residential.ToString())
                {
                    PropertList = _ResidentialService.GetResidentials();

                }
                else if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Commercial.ToString())
                {
                    PropertList = _CommercialService.GetCommercials();
                }
                else if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Condo.ToString())
                {
                    PropertList = _CondoService.GetCondos();
                }
                else
                {
                    PropertList = null;
                }
                var pager = new Pager(PropertList.Count(), page);

                var viewModel = new MainModel
                {
                    PropertiesModel = PropertList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                    Pager = pager,
                    CurrentPropertyType = CurrentPropertyType

                };

                string viewContent = ConvertViewToString("~/Views/WebPartial/_InnerListing.cshtml", viewModel);
                return Json(new { PartialView = viewContent });
            }
            catch (Exception)
            {

                return Json("error");
            }
           
        }

        public JsonResult RefreshPagination(int? page, string CurrentPropertyType)
        {
            try
            {
                List<PropertyModel> PropertList = new List<PropertyModel>();

                if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Residential.ToString())
                {
                    PropertList = _ResidentialService.GetResidentials();

                }
                else if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Commercial.ToString())
                {
                    PropertList = _CommercialService.GetCommercials();
                }
                else if (CurrentPropertyType != "" && CurrentPropertyType == EnumValue.PropertyType.Condo.ToString())
                {
                    PropertList = _CondoService.GetCondos();
                }
                else
                {
                    PropertList = null;
                }
                var pager = new Pager(PropertList.Count(), page);

                var viewModel = new MainModel
                {
                    PropertiesModel = PropertList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                    Pager = pager
                };

                string viewContent = ConvertViewToString("~/Views/WebPartial/_ListPaging.cshtml", viewModel);
                return Json(new { PartialView = viewContent });
            }
            catch (Exception)
            {

                return Json("error");
            }
           
        }

        //get single property  detail
        [HttpGet]
        public ActionResult GetPropertyDetail(string PropertyType,string MLS)
        {
            try
            {

                PropertyModel model = new PropertyModel();
                if (PropertyType != "" && PropertyType == EnumValue.PropertyType.Residential.ToString())
                {
                    model = _ResidentialService.GetSingleProperty(MLS);  
                }
                else if (PropertyType != "" && PropertyType == EnumValue.PropertyType.Commercial.ToString())
                {
                    model = _CommercialService.GetSingleProperty(MLS);
                }
                else if (PropertyType != "" && PropertyType == EnumValue.PropertyType.Condo.ToString())
                {
                    model = _CondoService.GetSingleProperty(MLS);
                }
                else
                {
                    model = null;
                }
                

                try
                {
                    string sourcePath;
                    if(model!=null)
                    {
                        var ImageRootPath = @"C:\MlsData\";//ConfigurationManager.AppSettings["ImageURL"].ToString(); ;
                        if (model.VOX == false)
                        {
                            sourcePath = ImageRootPath + "IDXResidential";
                        }
                        else
                        {
                            sourcePath = ImageRootPath + "VoxResidential";
                        }
                        List<PropertyImages> imagelist = new List<PropertyImages>();
                        DirectoryInfo dir = new DirectoryInfo(sourcePath);
                        if (model != null)
                        {
                            foreach (FileInfo files in dir.GetFiles("Photo" + model.MLS.ToString() + "*.*"))
                            {
                                PropertyImages image = new PropertyImages();
                                image.MLS = model.MLS.ToString();
                                image.Image = model.serverimagepath.ToString() + files.Name;
                                imagelist.Add(image);

                            }
                        }
                        model.PropertyImages = imagelist;
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("SearchProperty");
                        
                    }
                    
                   

                }
                catch (Exception ex)
                {

                    throw;
                }

               // return View(model);

            }
            catch (Exception ex)
            {

                throw;
                return View();
            }


        }

        private  string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }
    }
}