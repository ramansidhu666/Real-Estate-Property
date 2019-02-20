using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Property.Entity
{
   public class PropertyModel
    {
        public string Address { get; set; }
        public string AirConditioning { get; set; }
        public string Area { get; set; }
        public string Basement1 { get; set; }
        public string Bedrooms { get; set; }
        public string Community { get; set; }
        public string GarageType { get; set; }
        public string IdxUpdtedDt { get; set; }
        public string Kitchens { get; set; }
        public string ListBrokerage { get; set; }
        public string ListPrice { get; set; }
        public string MLS { get; set; }
        public string Municipality { get; set; }
        public string MunicipalityDistrict { get; set; }
        public string ParkingSpaces { get; set; }
        public string pImage { get; set; }
        public string Pool { get; set; }
        public string PostalCode { get; set; }
        public string RemarksForClients { get; set; }
        public string Rooms { get; set; }
        public string SaleLease { get; set; }
        public string Status { get; set; }
        public string Street { get; set; }
        public string StreetAbbreviation { get; set; }
        public string StreetDirection { get; set; }
        public string StreetName { get; set; }
        public string Style { get; set; }
        public string Taxes { get; set; }
        public string UtilitiesCable { get; set; }
        public string UtilitiesGas { get; set; }
        public string Washrooms { get; set; }
        public string Water { get; set; }
        public string Zoning { get; set; }
        public string VOX { get; set; }
        public string serverimagepath { get; set; }
       
        public List<PropertyImages> PropertyImages { get; set; }
    }

    public class PropertyImages
    {
        public string MLS { get; set; }
        public string Image { get; set; }
    }
}
