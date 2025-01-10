using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoCurrencyApi.Domain.Entities;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace CryptoCurrencyApi.Application.Services
{
    /// <summary>
    /// 幣別服務，處理幣別相關的商業邏輯
    /// </summary>
    public class CurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ICurrencyRepository currencyRepository,
                               ILogger<CurrencyService> logger)
        {
            _currencyRepository = currencyRepository 
                ?? throw new ArgumentNullException(nameof(currencyRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 新增幣別
        /// </summary>
        public async Task<CurrencyDto> AddCurrencyAsync(CreateCurrencyDto createDto)
        {
            // 驗證輸入
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            // 建立領域實體
            var currency = new Currency(createDto.Code, createDto.ChineseName);

            // 呼叫倉儲新增
            var addedCurrency = await _currencyRepository.AddAsync(currency);

            // 轉換為 DTO 回傳
            return new CurrencyDto
            {
                Code = addedCurrency.Code,
                ChineseName = addedCurrency.ChineseName,
                CreatedAt = addedCurrency.CreatedAt
            };
        }

        /// <summary>
        /// 取得幣別
        /// </summary>
        public async Task<CurrencyDto> GetCurrencyByCodeAsync(string code)
        {
            var currency = await _currencyRepository.GetByCodeAsync(code);

            if (currency == null)
                throw new KeyNotFoundException($"未找到代碼為 {code} 的幣別");

            return new CurrencyDto
            {
                Code = currency.Code,
                ChineseName = currency.ChineseName,
                CreatedAt = currency.CreatedAt,
                UpdatedAt = currency.UpdatedAt
            };
        }

        /// <summary>
        /// 更新幣別
        /// </summary>
        public async Task UpdateCurrencyAsync(string code, string chineseName)
        {
            _logger.LogInformation($"Updating currency {code} with name {chineseName}");
            
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("幣別代碼不能為空", nameof(code));

            // Check if currency exists and log result
            var currency = await _currencyRepository.GetByCodeAsync(code);
            _logger.LogInformation($"Currency found: {currency != null}");

            if (currency == null)
                throw new KeyNotFoundException($"未找到代碼為 {code} 的幣別");

            currency.Update(chineseName);
            await _currencyRepository.UpdateAsync(currency);
        }

        /// <summary>
        /// 刪除幣別
        /// </summary>
        public async Task DeleteCurrencyAsync(string code)
        {
            await _currencyRepository.DeleteAsync(code);
        }

        /// <summary>
        /// 列出所有幣別
        /// </summary>
        public async Task<IEnumerable<CurrencyDto>> ListCurrenciesAsync()
        {
            var currencies = await _currencyRepository.ListAsync();
            
            var currencyDtos = new List<CurrencyDto>();
            foreach (var currency in currencies)
            {
                currencyDtos.Add(new CurrencyDto
                {
                    Code = currency.Code,
                    ChineseName = currency.ChineseName,
                    CreatedAt = currency.CreatedAt,
                    UpdatedAt = currency.UpdatedAt
                });
            }

            return currencyDtos;
        }
    }
}