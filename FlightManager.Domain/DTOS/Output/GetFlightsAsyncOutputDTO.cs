using System.Collections.Generic;

namespace FlightManager.Domain.DTOS.Output
{
	public class GetFlightsAsyncOutputDTO
	{
        public List<FlightDTO> Flights { get; set; }
	}
}

