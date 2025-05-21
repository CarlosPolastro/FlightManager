using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlightManager.Domain.DTOS;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FlightManager.Web
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<MessageOutputDTO<T>> GetDataAsync<T>(string endpoint)
        {
            // Use named client "MyApiClient" configured in Startup
            var client = _httpClientFactory.CreateClient("LocalApiClient");
            MessageOutputDTO<T> output = new MessageOutputDTO<T>();

            try
            {
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode(); // Throws if not successful
                var json = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<MessageOutputDTO<T>>(json);
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the error as needed
                output.Errors.Add(ex.Message);
            }

            if (!output.Success)
                _logger.LogError(string.Join(";", output.Errors));

            return output;
        }

        public async Task<MessageOutputDTO<T2>> PostDataAsync<T1,T2>(string endpoint, T1 item)
        {
            var client = _httpClientFactory.CreateClient("LocalApiClient");
            MessageOutputDTO<T2> output = new MessageOutputDTO<T2>();

            try
            {
                // Serialize the data to JSON
                var jsonContent = JsonConvert.SerializeObject(item);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send POST request
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                var json =  await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<MessageOutputDTO<T2>>(json);

                if (!output.Success)
                    _logger.LogError(string.Join(";",output.Errors));

                return output;
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the error as needed
                output.Errors.Add(ex.Message);
            }

            if (!output.Success)
                _logger.LogError(string.Join(";", output.Errors));

            return output;
        }

        public async Task<MessageOutputDTO<T2>> PutDataAsync<T1, T2>(string endpoint, T1 item)
        {
            var client = _httpClientFactory.CreateClient("LocalApiClient");
            MessageOutputDTO<T2> output = new MessageOutputDTO<T2>();

            try
            {
                // Serialize the data to JSON
                var jsonContent = JsonConvert.SerializeObject(item);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send POST request
                var response = await client.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                var json =  await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<MessageOutputDTO<T2>>(json);

                if (!output.Success)
                    _logger.LogError(string.Join(";", output.Errors));

                return output;
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the error as needed
                output.Errors.Add(ex.Message);
            }

            if (!output.Success)
                _logger.LogError(string.Join(";", output.Errors));

            return output;
        }

        public async Task<MessageOutputDTO<T>> DeleteDataAsync<T>(string endpoint)
        {
            var client = _httpClientFactory.CreateClient("LocalApiClient");
            MessageOutputDTO<T> output = new MessageOutputDTO<T>();

            try
            {
                // Send DELETE request
                var response = await client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<MessageOutputDTO<T>>(json);

                if (!output.Success)
                    _logger.LogError(string.Join(";", output.Errors));

                return output;
            }
            catch (HttpRequestException ex)
            {
                // Handle or log the error as needed
                output.Errors.Add(ex.Message);
            }

            if (!output.Success)
                _logger.LogError(string.Join(";", output.Errors));

            return output;
        }
    }
}

