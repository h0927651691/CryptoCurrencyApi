using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CryptoCurrencyApi.Infrastructure.Data;
using CryptoCurrencyApi.Tests.Helpers;

namespace CryptoCurrencyApi.Tests.Integration
{
    public class ErrorHandlingTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ErrorHandlingTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }
        /// <summary>
        /// 測試非法請求的錯誤處理
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var invalidData = new { };  // 空物件
            var content = new StringContent(
                JsonSerializer.Serialize(invalidData),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/Currency", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        ///  測試外部 API 失敗的處理
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ExternalApiFailure_ReturnsInternalServerError()
        {
            _factory.SetMockHttpClientShouldFail(true);
            var response = await _client.GetAsync("/api/Coindesk/original");
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}