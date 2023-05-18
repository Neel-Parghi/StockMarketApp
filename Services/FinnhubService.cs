using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Services
{

    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            // Create Http Client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // Create Http Request
            HttpRequestMessage httpRequestMessage = new()
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                Method = HttpMethod.Get
            };

            // send Request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            // send response
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            // Convert responseBody to Dictionary from Json
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No Response from Server");

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            // Create Client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            // Http Request
            HttpRequestMessage httpRequestMessage = new()
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                Method = HttpMethod.Get
            };

            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            string ResponseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            Dictionary<string, object>? ResponseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ResponseBody);

            if (ResponseDictionary == null)
                throw new InvalidOperationException("No Response from server");
            if (ResponseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(ResponseDictionary["error"]));
            return ResponseDictionary;
        }
    }
}
