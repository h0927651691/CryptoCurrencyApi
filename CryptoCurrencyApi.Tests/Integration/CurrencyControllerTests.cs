using Microsoft.Extensions.DependencyInjection;
using CryptoCurrencyApi.Infrastructure.Data;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using CryptoCurrencyApi.Application.DTOs;
using CryptoCurrencyApi.Tests.Helpers;
using CryptoCurrencyApi.Domain.Entities;

public class CurrencyControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;


    public CurrencyControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

    }

    /// <summary>
    /// 測試取得所有貨幣
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAll_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var expectedMediaType = "application/json";

        // Act
        var response = await _client.GetAsync("/api/Currency");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(expectedMediaType, 
            response.Content.Headers.ContentType?.MediaType);
    }

    /// <summary>
    /// 測試新增貨幣
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResponse()
    {
        // Arrange
        var currency = new CreateCurrencyDto 
        { 
            Code = "USD", 
            ChineseName = "美元" 
        };
        var content = new StringContent(
            JsonSerializer.Serialize(currency),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/Currency", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// 測試不存在的貨幣代碼
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByCode_WithInvalidCode_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/Currency/INVALID");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}