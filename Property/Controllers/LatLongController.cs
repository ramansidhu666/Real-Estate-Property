using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Property.Core.UtilityManager;
using Property.Service;
using System.Threading;

namespace Property.Controllers
{
    public class LatLongController : Controller
    {
        private IIdxCommercialService _CommercialService { get; set; }
        private IIdxResidentialService _ResidentialService { get; set; }
        private IIdxCondoService _CondoService { get; set; }
        public LatLongController(IIdxCommercialService CommercialService, IIdxResidentialService ResidentialService, IIdxCondoService CondoService)
        {
            this._CommercialService = CommercialService;
            this._ResidentialService = ResidentialService;
            this._CondoService = CondoService;
        }
        public ActionResult index()
        {
            var ResidentialList = _ResidentialService.GetResidentials().Take(20);

            List<double> latlong = new List<double>();
            foreach (var item in ResidentialList)
            {
                Thread.Sleep(500);
                var Points = GoogleOperation.GetLatLong(item.Address);
                latlong.Add((double)Points[0]);
                latlong.Add((double)Points[1]);

            }

            

            return View();
        }
    }
           
}