using System.Collections.Generic;
using System.Threading.Tasks;
using FlightManager.Business.Interfaces;
using FlightManager.Data.Interfaces;
using FlightManager.Domain.DTOS;

namespace FlightManager.Business
{
    public class FlightService : IFlightService
    {
        IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<MessageOutputDTO<List<FlightDTO>>> GetFlightsAsync()
        {
            return await _flightRepository.GetFlightsAsync();
        }

        public async Task<MessageOutputDTO<FlightDTO>> GetFlightByIdAsync(decimal id)
        {
            return await _flightRepository.GetFlightByIdAsync(id);
        }

        public async Task<MessageOutputDTO<PagedResult<FlightDTO>>> GetPagedFlightsAsync(int page, int pageSize)
        {
            return await _flightRepository.GetPagedFlightsAsync(page, pageSize);
        }

        public async Task<MessageOutputDTO<int>> InsertFlightsAsync(FlightDTO flight)
        {
            return await _flightRepository.InsertFlightsAsync(flight);
        }

        public async Task<MessageOutputDTO<int>> UpdateFlightsAsync(FlightDTO flight)
        {
            return await _flightRepository.UpdateFlightsAsync(flight);
        }

        public async Task<MessageOutputDTO<int>> DeleteFlightAsync(decimal id)
        {
            return await _flightRepository.DeleteFlightAsync(id);
        }

        public async Task<MessageOutputDTO<List<AircraftTypeDTO>>> GetAircarftTypesAsync()
        {
            return await _flightRepository.GetAircarftTypesAsync();
        }

        public async Task<MessageOutputDTO<AircraftTypeDTO>> GetAircarftTypeByIdAsync(int id)
        {
            return await _flightRepository.GetAircarftTypeByIdAsync(id);
        }

        public async Task<MessageOutputDTO<List<AirportDTO>>> GetAirportsAsync()
        {
            return await _flightRepository.GetAirportsAsync();
        }

        public async Task<MessageOutputDTO<AirportDTO>> GetAirportByIdAsync(int id)
        {
            return await _flightRepository.GetAirportByIdAsync(id);
        }

        public async Task<MessageOutputDTO<FlightDTO>> CalculateFlightInfo(FlightDTO flight)
        {
            var output = new MessageOutputDTO<FlightDTO>();

            var outputDeparture = await GetAirportByIdAsync(flight.DepartureAirportID.Value);
            var airportDeparute = outputDeparture.Data;

            var outputDestination = await GetAirportByIdAsync(flight.DestinationAirportID.Value);
            var airportDestination = outputDestination.Data;


            var outputAircraft = await GetAircarftTypeByIdAsync(flight.AircraftTypeID.Value);
            var aircarft = outputAircraft.Data;

            flight.DistanceKm = Helper.GpsDistanceCalculator.CalculateDistance(airportDeparute.Latitude,
                airportDeparute.Longitude,
                airportDestination.Latitude,
                airportDestination.Longitude);

            flight.TotalFuelLiter = (flight.DistanceKm * aircarft.MaxFuelUsageLiterPerKm) + aircarft.MaxFuelTakeoffEffortLiter;

            output.Data = flight;

            return output;
        }

    }
}

