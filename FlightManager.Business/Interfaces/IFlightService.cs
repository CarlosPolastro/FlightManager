using FlightManager.Domain.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManager.Business.Interfaces
{
	public interface IFlightService
	{
        public Task<MessageOutputDTO<List<FlightDTO>>> GetFlightsAsync();
        public Task<MessageOutputDTO<FlightDTO>> GetFlightByIdAsync(decimal id);
        public Task<MessageOutputDTO<PagedResult<FlightDTO>>> GetPagedFlightsAsync(int page, int pageSize);
        public Task<MessageOutputDTO<int>> InsertFlightsAsync(FlightDTO flight);
        public Task<MessageOutputDTO<int>> UpdateFlightsAsync(FlightDTO flight);
        public Task<MessageOutputDTO<int>> DeleteFlightAsync(decimal id);
        public Task<MessageOutputDTO<List<AircraftTypeDTO>>> GetAircarftTypesAsync();
        public Task<MessageOutputDTO<List<AirportDTO>>> GetAirportsAsync();
        public Task<MessageOutputDTO<FlightDTO>> CalculateFlightInfo(FlightDTO flight);
    }
}

