using System.ComponentModel.DataAnnotations;
using FlightManager.Domain.DataAnnotations;

namespace FlightManager.Domain.DTOS
{
	public class FlightDTO
	{
		public decimal ID { get; set; }

        [Required(ErrorMessage = "Please select a departure airport.")]
        [NotEqual("DestinationAirportID", ErrorMessage = "Departure and Destination airports must be differents.")]
        public int? DepartureAirportID { get; set; }
        [Required(ErrorMessage = "Please select a destination airport.")]
        [NotEqual("DepartureAirportID", ErrorMessage = "Destination and Departure airport must be differents.")]
        public int? DestinationAirportID { get; set; }
        [Required(ErrorMessage = "Please select the aircraft type.")]
        public int? AircraftTypeID { get; set; }
        
        public double DistanceKm { get; set; }
        public double TotalFuelLiter { get; set; }

        public string DepartureAirport { get; set; }
        public string DestinationAirport { get; set; }
        public string AircraftTypeDescription { get; set; }
    }
}

