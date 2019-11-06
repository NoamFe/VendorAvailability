using System;
using System.Collections.Generic;
using System.Text;

namespace VendorAvailabilitySystem
{
    public class Meal
    {
        public int VendorId { get; set; }
        public int ClientId { get; set; }
        public DateTime Datetime { get; set; }
    }

    public class MealsResponse
    {
        public List<Meal> Results { get; set; }
    }

    public class VendorDrivers
    {
        public int VendorId { get; set; }
        public int Drivers { get; set; }
    }

    public class VendorResponse
    {
        public List<VendorDrivers> Results { get; set; }
    }
}
