using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FlightManager.Data.Interfaces;
using FlightManager.Domain.DTOS;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlightManager.Data
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ILogger<FlightRepository> _logger;
        private readonly string _connectionString;

        public FlightRepository(IConfiguration configuration, ILogger<FlightRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<MessageOutputDTO<List<FlightDTO>>> GetFlightsAsync()
        {
            var output = new MessageOutputDTO<List<FlightDTO>>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    F.ID,
                    A1.ID AS DepartureAirportID,
                    A1.Name AS DepartureAirport,
                    A2.ID AS DestinationAirportID,
                    A2.Name AS DestinationAirport,
                    AT.ID AS AircraftTypeID,
                    AT.Description AS AircraftTypeDescription,
                    F.DistanceKm,
                    F.TotalFuelLiter
                FROM Flights F
                INNER JOIN Airports A1 ON F.DepartureAirportID = A1.ID
                INNER JOIN Airports A2 ON F.DestinationAirportID = A2.ID
                INNER JOIN AircraftTypes AT ON F.AircraftTypeID = AT.ID";

                var flights = await connection.QueryAsync<FlightDTO>(query);

                output.Data = flights.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<FlightDTO>> GetFlightByIdAsync(decimal id)
        {
            var output = new MessageOutputDTO<FlightDTO>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    F.ID,
                    A1.ID AS DepartureAirportID,
                    A1.Name AS DepartureAirport,
                    A2.ID AS DestinationAirportID,
                    A2.Name AS DestinationAirport,
                    AT.ID AS AircraftTypeID,
                    AT.Description AS AircraftTypeDescription,
                    F.DistanceKm,
                    F.TotalFuelLiter
                FROM Flights F
                INNER JOIN Airports A1 ON F.DepartureAirportID = A1.ID
                INNER JOIN Airports A2 ON F.DestinationAirportID = A2.ID
                INNER JOIN AircraftTypes AT ON F.AircraftTypeID = AT.ID
                WHERE F.ID = @Id";

                var flight = await connection.QueryFirstAsync<FlightDTO>(query, new { Id = id });

                output.Data = flight;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<PagedResult<FlightDTO>>> GetPagedFlightsAsync(int page, int pageSize)
        {
            var output = new MessageOutputDTO<PagedResult<FlightDTO>>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var offset = (page - 1) * pageSize;

                var sql = @"
                    SELECT COUNT(*) FROM Flights;

                    SELECT f.ID, a1.Name AS DepartureAirport, a2.Name AS DestinationAirport, ac.Description AS AircraftTypeDescription,
                           f.DistanceKm, f.TotalFuelLiter
                    FROM Flights f
                    JOIN Airports a1 ON f.DepartureAirportID = a1.ID
                    JOIN Airports a2 ON f.DestinationAirportID = a2.ID
                    JOIN AircraftTypes ac ON f.AircraftTypeID = ac.ID
                    ORDER BY f.ID
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                ";

                using var multi = await connection.QueryMultipleAsync(sql, new { Offset = offset, PageSize = pageSize });
                var totalCount = await multi.ReadSingleAsync<int>();
                var items = (await multi.ReadAsync<FlightDTO>()).ToList();

                output.Data = new PagedResult<FlightDTO>
                {
                    Items = items,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<int>> InsertFlightsAsync(FlightDTO flight)
        {
            var output = new MessageOutputDTO<int>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO Flights (
                                AircraftTypeID,
                                DepartureAirportID ,
                                DestinationAirportID ,
                                DistanceKm,
                                TotalFuelLiter)
                            OUTPUT INSERTED.Id
                                VALUES (
                                @AircraftTypeID,
                                @DepartureAirportID,
                                @DestinationAirportID,
                                @DistanceKm,
                                @TotalFuelLiter
                            )";
                output.Data = await connection.ExecuteScalarAsync<int>(sql, flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<int>> UpdateFlightsAsync(FlightDTO flight)
        {
            var output = new MessageOutputDTO<int>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"UPDATE Flights SET
                                AircraftTypeID = @AircraftTypeID,
                                DepartureAirportID = @DepartureAirportID ,
                                DestinationAirportID = @DestinationAirportID ,
                                DistanceKm = @DistanceKm,
                                TotalFuelLiter = @TotalFuelLiter
                            WHERE Id = @ID";
                output.Data = await connection.ExecuteAsync(sql, flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<int>> DeleteFlightAsync(decimal id)
        {
            var output = new MessageOutputDTO<int>();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                string query = @"DELETE FROM Flights WHERE ID = @Id";

                output.Data = await connection.ExecuteAsync(query, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<List<AircraftTypeDTO>>> GetAircarftTypesAsync()
        {
            var output = new MessageOutputDTO<List<AircraftTypeDTO>>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    ID,
                    Description,
                    PassangerCapacity,
                    MaxFuelUsageLiterPerKm,
                    MaxFuelUsageLiterPerHour,
                    MaxFuelTakeoffEffortLiter
                FROM AircraftTypes";

                var aircraftTypes = await connection.QueryAsync<AircraftTypeDTO>(query);

                output.Data = aircraftTypes.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<AircraftTypeDTO>> GetAircarftTypeByIdAsync(int id)
        {
            var output = new MessageOutputDTO<AircraftTypeDTO>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    ID,
                    Description,
                    PassangerCapacity,
                    MaxFuelUsageLiterPerKm,
                    MaxFuelUsageLiterPerHour,
                    MaxFuelTakeoffEffortLiter
                FROM AircraftTypes
                WHERE ID = @Id";

                var aircraftType = await connection.QueryFirstAsync<AircraftTypeDTO>(query, new { Id = id});

                output.Data = aircraftType;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<List<AirportDTO>>> GetAirportsAsync()
        {
            var output = new MessageOutputDTO<List<AirportDTO>>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    ID,
                    Name ,
                    IATACode ,
                    ICAOCode,
                    Latitude ,
                    Longitude 
                FROM Airports";

                var airports = await connection.QueryAsync<AirportDTO>(query);

                output.Data = airports.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }

        public async Task<MessageOutputDTO<AirportDTO>> GetAirportByIdAsync(int id)
        {
            var output = new MessageOutputDTO<AirportDTO>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                //var users = await context.Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
                string query = @"
                SELECT
                    ID,
                    Name ,
                    IATACode ,
                    ICAOCode,
                    Latitude ,
                    Longitude 
                FROM Airports
                WHERE ID = @Id";

                var airport = await connection.QueryFirstAsync<AirportDTO>(query, new { Id = id });

                output.Data = airport;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                output.Errors.Add(ex.Message);
            }

            return output;
        }
    }
}

