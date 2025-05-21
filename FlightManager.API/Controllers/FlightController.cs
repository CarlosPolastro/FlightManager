using System.Threading.Tasks;
using FlightManager.Business.Interfaces;
using FlightManager.Domain.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private IFlightService _flightService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightService flightService, ILogger<FlightController> logger)
        {
            _flightService = flightService;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves a list of all flights.
        /// </summary>
        /// <returns>A list of flights.</returns>
        [HttpGet("GetFlightsAsync")]
        public async Task<IActionResult> GetFlightsAsync()
        {
            return Ok(await _flightService.GetFlightsAsync());
        }

        /// <summary>
        /// Retrieves a flight by ID.
        /// </summary>
        /// <returns>Flight</returns>
        [HttpGet("GetFlightByIdAsync/{id}")]
        public async Task<IActionResult> GetFlightByIdAsync(decimal id)
        {
            return Ok(await _flightService.GetFlightByIdAsync(id));
        }

        /// <summary>
        /// Retrieves a list of paged flights.
        /// </summary>
        /// <param name="page">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns></returns>
        [HttpGet("GetFlightsPagedAsync")]
        public async Task<IActionResult> GetFlightsPagedAsync(int page = 1, int pageSize = 10)
        {
            var pagedFlights = await _flightService.GetPagedFlightsAsync(page, pageSize);
            return Ok(pagedFlights);
        }


        /// <summary>
        /// Insert a flight.
        /// </summary>
        /// <returns>ID of inserted flight</returns>
        [HttpPost("InsertFlightsAsync")]
        public async Task<IActionResult> InsertFlightsAsync(FlightDTO flight)
        {
            return Ok(await _flightService.InsertFlightsAsync(flight));
        }

        /// <summary>
        /// Update a flight.
        /// </summary>
        /// <returns>Result: true/false</returns>
        [HttpPut("UpdateFlightsAsync")]
        public async Task<IActionResult> UpdateFlightsAsync(FlightDTO flight)
        {
            return Ok(await _flightService.UpdateFlightsAsync(flight));
        }


        /// <summary>
        /// Delete a flight by ID.
        /// </summary>
        /// <returns>Flight</returns>
        [HttpDelete("DeleteFlightAsync/{id}")]
        public async Task<IActionResult> DeleteFlightAsync(decimal id)
        {
            return Ok(await _flightService.DeleteFlightAsync(id));
        }

        /// <summary>
        /// Retrieves a list of all aircarfts.
        /// </summary>
        /// <returns>A list of aircarfts.</returns>
        [HttpGet("GetAircarftTypesAsync")]
        public async Task<IActionResult> GetAircarftTypesAsync()
        {
            return Ok(await _flightService.GetAircarftTypesAsync());
        }

        /// <summary>
        /// Retrieves a list of all airports.
        /// </summary>
        /// <returns>A list of airports.</returns>
        [HttpGet("GetAirportsAsync")]
        public async Task<IActionResult> GetAirportsAsync()
        {
            return Ok(await _flightService.GetAirportsAsync());
        }

        [HttpPut("CalculateFlightInfo")]
        public async Task<IActionResult> CalculateFlightInfo(FlightDTO flight)
        {
            return Ok(await _flightService.CalculateFlightInfo(flight));
        }
    }
}

