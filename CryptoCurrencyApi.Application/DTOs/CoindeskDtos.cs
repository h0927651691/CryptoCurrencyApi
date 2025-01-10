using System.Text.Json.Serialization;

namespace CryptoCurrencyApi.Application.DTOs
{
    /// <summary>
    /// Coindesk API 回應 DTO
    /// </summary>
    public class CoindeskApiResponse
    {
        [JsonPropertyName("time")]
        public Time Time { get; set; }

        [JsonPropertyName("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonPropertyName("chartName")]  
        public string ChartName { get; set; }

        [JsonPropertyName("bpi")]
        public Bpi Bpi { get; set; }
    }

    public class Time
    {
        [JsonPropertyName("updated")]
        public string Updated { get; set; }
        [JsonPropertyName("updatedISO")]
        public DateTime UpdatedISO { get; set; }
        [JsonPropertyName("updateduk")]

        public string Updateduk { get; set; }
    }
    public class Bpi
    {
        [JsonPropertyName("USD")]
        public CurrencyInfo USD { get; set; }

        [JsonPropertyName("GBP")]
        public CurrencyInfo GBP { get; set; }

        [JsonPropertyName("EUR")]
        public CurrencyInfo EUR { get; set; }
    }

    public class CurrencyInfo
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("rate_float")]
        public decimal Rate_float { get; set; }
    }


    /// <summary>
    /// 自訂匯率資訊回應 DTO
    /// </summary>
    public class CustomRateResponse
    {
        public string UpdateTime { get; set; }
        public List<CurrencyRate> currencies { get; set; } = new();
    }

    public class CurrencyRate
    {
        public string Code { get; set; }
        public string ChineseName { get; set; }
        public decimal Rate { get; set; }
    }
}