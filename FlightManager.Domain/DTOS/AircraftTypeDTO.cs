namespace FlightManager.Domain.DTOS
{
    public class AircraftTypeDTO
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int PassangerCapacity { get; set; }
        public double MaxFuelUsageLiterPerKm { get; set; }
        public double MaxFuelUsageLiterPerHour { get; set; }
        public double MaxFuelTakeoffEffortLiter { get; set; }
    }
}

