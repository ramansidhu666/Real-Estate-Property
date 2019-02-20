using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Entity
{
    public class SearchModel
    {
        
        public string MLSID_City_PostalCode { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string BedRooms { get; set; }
        public string BathRooms { get; set; }
        public string PropertyType { get; set; }
        public string SaleLease { get; set; }
        
    }
}