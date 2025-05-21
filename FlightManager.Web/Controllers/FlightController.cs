using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FlightManager.Domain.DTOS;
using FlightManager.Models;
using FlightManager.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FlightManager.Web.Controllers
{
	public class FlightController : Controller
    {
        private const string _apiPath = "api/Flight";
        private readonly ApiService _apiService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(ApiService apiService, ILogger<FlightController> logger)
        {

            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var flight = new FlightDTO();

            await LoadViewBags(page, pageSize);

            return View(flight);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int? id)
        {
            var output = await _apiService.GetDataAsync<FlightDTO>($"{_apiPath}/GetFlightByIdAsync/{id}");

            if (!output.Success)
            {
                _logger.LogError(string.Join(";", output.Errors));
                ViewBag.ErrorMessage = "An error ocurred during retrieving flight data. Try again later.";
            }

            await LoadViewBags();

            var flight = output.Data ?? new FlightDTO();

            return View("Index", flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(FlightDTO flight)
        {   
            if (ValidateSubmittedData(flight))
            {
                var outputInsert = await _apiService.PostDataAsync<FlightDTO, int>($"{_apiPath}/InsertFlightsAsync", flight);

                if (!outputInsert.Success)
                {
                    _logger.LogError("An error ocurred on Insert flight: {Error} ", string.Join(";", outputInsert.Errors));
                    ViewBag.ErrorMessage = "An error ocurred on Insert flight. Try again later.";
                }
                else
                {
                    ViewBag.SuccessMessage = "Flight added successfully!";
                    _logger.LogInformation("Flight with ID {FlightId} added successfully", outputInsert.Data);
                    flight = new FlightDTO();
                }
            }

            await LoadViewBags();
            return View("Index", flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlightDTO flight)
        {
            if (ValidateSubmittedData(flight))
            {
                var outputUpdate = await _apiService.PutDataAsync<FlightDTO, bool>($"{_apiPath}/UpdateFlightsAsync", flight);

                if (!outputUpdate.Success)
                {
                    _logger.LogError("An error ocurred on Update flight: {Error} ", string.Join(";", outputUpdate.Errors));
                    ViewBag.ErrorMessage = "An error ocurred on Update flight. Try again later.";
                }
                else
                {
                    ViewBag.SuccessMessage = "Flight updated successfully!";
                    _logger.LogInformation("Flight with ID {FlightId} updated successfully", flight.ID);
                    flight = new FlightDTO();
                }
            }

            await LoadViewBags();
            return View("Index", flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var outputDelete = await _apiService.DeleteDataAsync<int>($"{_apiPath}/DeleteFlightAsync/{id}");

            if (!outputDelete.Success)
            {
                _logger.LogError("An error ocurred on Delete flight: {Error} ", string.Join(";", outputDelete.Errors));
                ViewBag.ErrorMessage = "An error ocurred on Delete flight. Try again later.";
            }
            else
            {
                ViewBag.SuccessMessage = "Flight deleted successfully!";
                _logger.LogInformation("Flight with ID {FlightId} deleted successfully", id);
            }

            await LoadViewBags();
            return View("Index", new FlightDTO());
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateFlightInfo(FlightDTO flight)
        {
            if (ValidateSubmittedData(flight))
            {
                var outputCalculate = await _apiService.PutDataAsync<FlightDTO, FlightDTO>($"{_apiPath}/CalculateFlightInfo", flight);

                if (!outputCalculate.Success)
                {
                    _logger.LogError("An error ocurred during Calculate flight info: {Error}", string.Join(";", outputCalculate.Errors));
                    ViewBag.ErrorMessage = "An error ocurred during Calculate flight info. Try again later.";
                }
                else
                {
                    _logger.LogInformation("CalculateFlightInfo succeeded: DistanceKm={Distance}, TotalFuelLiter={Fuel}",
                            outputCalculate.Data.DistanceKm, outputCalculate.Data.TotalFuelLiter);
                    flight = outputCalculate.Data;
                }
            }

            await LoadViewBags();
            // Return the partial view
            return PartialView("_FlightCalculatePartial", flight);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task LoadViewBags(int page = 1, int pageSize = 10)
        {
            var outputAircrafts = await _apiService.GetDataAsync<List<AircraftTypeDTO>>($"{_apiPath}/GetAircarftTypesAsync");
            ViewBag.AircraftTypes = outputAircrafts.Data.DefaultIfNull();
            ViewBag.AircraftTypesJson = JsonConvert.SerializeObject(outputAircrafts.Data.DefaultIfNull());
            if (!outputAircrafts.Success)
            {
                _logger.LogError(string.Join(";", outputAircrafts.Errors));
                ViewBag.ErrorMessage = "An error occurred loading aircrafts list.";
            }

            var outputAirports = await _apiService.GetDataAsync<List<AirportDTO>>($"{_apiPath}/GetAirportsAsync");
            ViewBag.Airports = outputAirports.Data.DefaultIfNull();
            if (!outputAirports.Success)
            {
                _logger.LogError(string.Join(";", outputAirports.Errors));
                ViewBag.ErrorMessage = "An error occurred loading airports list.";
            }
       
            var outputFlightsPaged = await _apiService.GetDataAsync<PagedResult<FlightDTO>>($"{_apiPath}/GetFlightsPagedAsync?page={page}&pageSize={pageSize}");
            if (outputFlightsPaged.Success)
            {
                ViewBag.Flights = outputFlightsPaged.Data.Items;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = outputFlightsPaged.Data.TotalPages;
            }
            else
            {
                _logger.LogError(string.Join(";", outputFlightsPaged.Errors));
                ViewBag.Flights = new List<FlightDTO>();
                ViewBag.ErrorMessage = "An error occurred loading flights.";
            }   
        }

        public bool ValidateSubmittedData(FlightDTO flight)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Form submmission failed. Check data and try again.");
                return false;
            }
            else
            {
                if(flight.DepartureAirportID == flight.DestinationAirportID)
                {
                    ModelState.AddModelError("SameAirpoirts", "Departure and Destination Airport cannot be the same.");
                    return false;
                }
            }

            ModelState.Clear();

            return true;
        }
    }
}