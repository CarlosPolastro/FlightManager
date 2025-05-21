namespace FlightManager.Domain.DTOS
{
    public class AirportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string IATACode { get; set; }
        public string ICAOCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

