using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Application.DTOs;

namespace CryptoCurrencyApi.Application.Services
{
    public class CoindeskService : ICoindeskService
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CoindeskService> _logger;

        public CoindeskService(
            IHttpClientFactory httpClientFactory,
            ICurrencyRepository currencyRepository,
            ILogger<CoindeskService> logger)
        {
             _httpClient = httpClientFactory.CreateClient("CoinDeskApi");
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CoindeskApiResponse> GetCurrentPriceAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Coindesk API Response: {Response}", content);
                
                   return JsonSerializer.Deserialize<CoindeskApiResponse>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from Coindesk API");
                throw;
            }
        }

    public async Task<CustomRateResponse> GetCustomRateAsync()
    {
        var coindeskData = await GetCurrentPriceAsync();
        var currencies = await _currencyRepository.ListAsync();
        
        var response = new CustomRateResponse
        {
            UpdateTime = coindeskData.Time.UpdatedISO.ToString("yyyy/MM/dd HH:mm:ss"),
            currencies = new List<CurrencyRate>()
        };

        // 直接處理 Bpi 中的幣別資訊
        if (coindeskData.Bpi.USD != null)
        {
            var localCurrency = currencies.FirstOrDefault(c => c.Code.Equals(coindeskData.Bpi.USD.Code, StringComparison.OrdinalIgnoreCase));
            response.currencies.Add(new CurrencyRate 
            {
                Code = "USD",
                ChineseName = localCurrency?.ChineseName ?? "USD",
                Rate = coindeskData.Bpi.USD.Rate_float
            });
        }

        if (coindeskData.Bpi.GBP != null)
        {
            var localCurrency = currencies.FirstOrDefault(c => c.Code.Equals(coindeskData.Bpi.GBP.Code, StringComparison.OrdinalIgnoreCase));
            response.currencies.Add(new CurrencyRate 
            {
                Code = "GBP",
                ChineseName = localCurrency?.ChineseName ?? "GBP",
                Rate = coindeskData.Bpi.GBP.Rate_float
            });
        }

        if (coindeskData.Bpi.EUR != null)
        {
            var localCurrency = currencies.FirstOrDefault(c => c.Code.Equals(coindeskData.Bpi.EUR.Code, StringComparison.OrdinalIgnoreCase));
            response.currencies.Add(new CurrencyRate 
            {
                Code = "EUR",
                ChineseName = localCurrency?.ChineseName ?? "EUR",
                Rate = coindeskData.Bpi.EUR.Rate_float
            });
        }

        return response;
    }
    }
}