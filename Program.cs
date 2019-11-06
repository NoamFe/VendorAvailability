using System;
using System.Linq;

namespace VendorAvailabilitySystem
{
    class Program
    {
        static MealsResponse meals;
        static VendorResponse vendors;
        static int BlackOutMinutesBefore = 30;
        static int BlackOutMinutesAfter = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("welcome to the vendor availability application");

            meals = GetMeals();
            vendors = GetVendors();
            
            Console.WriteLine($"blackout time before is {BlackOutMinutesBefore} minutes");
            Console.WriteLine($"if you want to change it, enter a number, if not press enter");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                BlackOutMinutesBefore = Int32.Parse(input);

            Console.WriteLine($"blackout time after is {BlackOutMinutesAfter} minutes");
            Console.WriteLine($"if you want to change it, enter a number, if not press enter");
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                BlackOutMinutesAfter = Int32.Parse(input);

         
                         
            IsAvailable(1,new DateTime(2017, 1, 1, 13, 41, 0));
            IsAvailable(1,new DateTime(2017, 1, 1, 13, 59, 59));
            IsAvailable(1,new DateTime(2017, 1, 1, 14, 30, 0));
            IsAvailable(1,new DateTime(2017, 1, 1, 14, 39, 58));
            IsAvailable(1,new DateTime(2017, 1, 1, 14, 40, 2));
            IsAvailable(1,new DateTime(2017, 1, 1, 15, 0, 0));
            IsAvailable(1,new DateTime(2017, 1, 1, 15, 1, 0));


            IsAvailable(2,new DateTime(2017, 1, 1, 13, 30, 0));
            IsAvailable(2,new DateTime(2017, 1, 1, 15, 0, 0));
            IsAvailable(2,new DateTime(2017, 1, 1, 13, 41, 0));
            IsAvailable(2,new DateTime(2017, 1, 1, 15, 01, 0));             
        }

        private static void IsAvailable(int vendorId, DateTime time)
        { 
            var response = IsVendorAvailable(vendorId, time);
            Console.WriteLine($"for vendor {vendorId} at {time} , availability is: {response}");
        }

        public static bool IsVendorAvailable(int vendorId, DateTime dateTime)
        {
            var scheduledDeliveries = meals.Results.Where(e => e.VendorId == vendorId)
                .Select(e => e.Datetime).OrderBy(e=>e).ToList();

            if (scheduledDeliveries.Count == 0)
                return true;

            var driverCount = vendors.Results.Where(e => e.VendorId == vendorId)
                .Select(e => e.Drivers).FirstOrDefault();

            if (driverCount == 0)
                return false;

            foreach (var date in scheduledDeliveries)
            {
                var startBackOutTime = date.AddMinutes(-1* BlackOutMinutesBefore);
                var endBackOutTime = date.AddMinutes(BlackOutMinutesAfter);
                if (dateTime >= startBackOutTime && dateTime <= endBackOutTime)
                {
                    driverCount--;
                    if (driverCount == 0)
                        return false;

                }
            }

            return true;
        }

        static VendorResponse GetVendors()
        {
            VendorResponse response = new VendorResponse();

            response.Results = new System.Collections.Generic.List<VendorDrivers>();
            response.Results.Add(new VendorDrivers { VendorId = 1, Drivers = 1 });
            response.Results.Add(new VendorDrivers { VendorId = 2, Drivers = 3 });

            return response;
        }

        static MealsResponse GetMeals()
        {
            MealsResponse response = new MealsResponse();

            response.Results = new System.Collections.Generic.List<Meal>();
            response.Results.Add(new Meal { VendorId = 1 , ClientId = 10 , Datetime = new DateTime(2017,1,1,13,30,00)});
            response.Results.Add(new Meal { VendorId = 1, ClientId = 40, Datetime = new DateTime(2017, 1, 1, 14, 30, 00) });
            response.Results.Add(new Meal { VendorId = 2, ClientId = 20, Datetime = new DateTime(2017, 1, 1, 13, 30, 00) });

            return response;
        }
    }
}
