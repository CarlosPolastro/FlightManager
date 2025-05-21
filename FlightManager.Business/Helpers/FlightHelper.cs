using System;
namespace FlightManager.Business
{
	public static class Helper
	{
        public class GpsDistanceCalculator
        {
            private const double EarthRadiusKm = 6371.0; // Earth's radius in kilometers

            public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
            {
                // Convert latitude and longitude from degrees to radians
                double lat1Rad = DegreesToRadians(lat1);
                double lon1Rad = DegreesToRadians(lon1);
                double lat2Rad = DegreesToRadians(lat2);
                double lon2Rad = DegreesToRadians(lon2);

                // Differences in coordinates
                double deltaLat = lat2Rad - lat1Rad;
                double deltaLon = lon2Rad - lon1Rad;

                // Haversine formula
                double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                            Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                            Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double distance = EarthRadiusKm * c;

                return distance; // Distance in kilometers
            }

            private static double DegreesToRadians(double degrees)
            {
                return degrees * Math.PI / 180.0;
            }
        }
	}
}

