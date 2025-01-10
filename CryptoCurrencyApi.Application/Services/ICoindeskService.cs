using System.Threading.Tasks;
using CryptoCurrencyApi.Application.DTOs;

namespace CryptoCurrencyApi.Application.Services
{
    public interface ICoindeskService
    {
        Task<CoindeskApiResponse> GetCurrentPriceAsync();
        Task<CustomRateResponse> GetCustomRateAsync();
    }
}