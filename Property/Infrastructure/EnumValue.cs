using System;
using System.ComponentModel;
using System.Reflection;

namespace Property.Infrastructure
{
    public class EnumValue
    {
        public enum CustomerType{[Description("Super Admin")]SuperAdmin = 1, [Description("Admin")]Admin = 2, [Description("Basic")]Basic = 3, [Description("Professional")]Professional = 4}
        public enum Gender {[Description("Male")]Male= 1, [Description("Female")]Female = 2 }

        public enum EmailType {[Description("WelcomeEmail")]WelcomeEmail = 1, [Description("ResetPassword")]ResetPassword = 2, [Description("ReverifyAccount")]ReverifyAccount = 3 }
       
        public enum GoogleDistanceType { Km = 1, Miles = 2, Meters = 3 }
        public enum PropertyType { Residential = 1, Commercial = 2, Condo = 3 }
        public enum GoogleDistanceTypeInStr {[Description("km")]Km, [Description("miles")] Miles, [Description("meters")] Meters }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
