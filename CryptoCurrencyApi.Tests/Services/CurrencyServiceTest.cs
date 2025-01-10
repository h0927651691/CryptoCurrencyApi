using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CryptoCurrencyApi.Domain.Entities;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Application.Services;
using CryptoCurrencyApi.Application.DTOs;
using Microsoft.Extensions.Logging;
namespace CryptoCurrencyApi.Tests.Services
{
    public class CurrencyServiceTests
    {

    private readonly Mock<ICurrencyRepository> _mockRepository;
    private readonly Mock<ILogger<CurrencyService>> _mockLogger;
    private readonly CurrencyService _service;
        public CurrencyServiceTests()
        {
            _mockRepository = new Mock<ICurrencyRepository>();
            _mockLogger = new Mock<ILogger<CurrencyService>>();
            _service = new CurrencyService(_mockRepository.Object, _mockLogger.Object);
        }

        /// <summary>
        /// 測試新增貨幣服務
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddCurrencyAsync_WithValidData_ShouldReturnCurrencyDto()
        {
            // Arrange
            var createDto = new CreateCurrencyDto 
            { 
                Code = "USD", 
                ChineseName = "美元" 
            };

            var currency = new Currency("USD", "美元");
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Currency>()))
                .ReturnsAsync(currency);

            // Act
            var result = await _service.AddCurrencyAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Code, result.Code);
            Assert.Equal(createDto.ChineseName, result.ChineseName);
        }

        /// <summary>
        /// 測試取得貨幣
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetCurrencyByCodeAsync_WithExistingCode_ShouldReturnCurrencyDto()
        {
            // Arrange
            var currency = new Currency("USD", "美元");
            _mockRepository.Setup(r => r.GetByCodeAsync("USD"))
                .ReturnsAsync(currency);

            // Act
            var result = await _service.GetCurrencyByCodeAsync("USD");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Code);
            Assert.Equal("美元", result.ChineseName);
        }

        /// <summary>
        /// 測試找不到貨幣
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetCurrencyByCodeAsync_WithNonExistingCode_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByCodeAsync("XXX"))
                .ReturnsAsync((Currency)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetCurrencyByCodeAsync("XXX"));
        }

        /// <summary>
        /// 測試更新貨幣
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateCurrencyAsync_WithValidData_ShouldUpdateChineseName()
        {
            // Arrange
            var code = "USD";
            var newChineseName = "美金";
            
            var currency = new Currency("USD", "美元");
            _mockRepository.Setup(r => r.GetByCodeAsync(code))
                .ReturnsAsync(currency);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Currency>()))
                .ReturnsAsync(currency);

            // Act
            await _service.UpdateCurrencyAsync(code, newChineseName);

            // Assert 
            Assert.Equal(newChineseName, currency.ChineseName);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Currency>()), Times.Once);
        }

        /// <summary>
        /// 測試刪除貨幣
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteCurrencyAsync_WithExistingCode_ShouldCallRepository()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync("USD"))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCurrencyAsync("USD");

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync("USD"), Times.Once);
        }

        /// <summary>
        /// 測試列出所有貨幣
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ListCurrenciesAsync_ShouldReturnAllCurrencies()
        {
            // Arrange
            var currencies = new List<Currency> 
            {
                new Currency("USD", "美元"),
                new Currency("EUR", "歐元")
            };

            _mockRepository.Setup(r => r.ListAsync(It.IsAny<bool>()))
                .ReturnsAsync(currencies);

            // Act
            var result = await _service.ListCurrenciesAsync();

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(2, new List<CurrencyDto>(result).Count);
        }
    }
}