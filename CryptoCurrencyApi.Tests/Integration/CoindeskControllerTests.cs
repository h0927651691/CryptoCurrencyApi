using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CryptoCurrencyApi.Application.DTOs;
using CryptoCurrencyApi.Infrastructure.Data;
using CryptoCurrencyApi.Domain.Entities;
using CryptoCurrencyApi.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

public class CoindeskControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScope _scope;
    private readonly ApplicationDbContext _context;
    private readonly ITestOutputHelper _output;
    public CoindeskControllerTests(CustomWebApplicationFactory factory,
                                   ITestOutputHelper output)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _output = output;
        InitializeDatabase();
    }   
    private void InitializeDatabase()
    {
        // 確保先清空資料表
        _context.Currencies.RemoveRange(_context.Currencies);
        _context.SaveChanges();

        // 添加測試資料
        _context.Currencies.AddRange(
            new Currency { Code = "USD", ChineseName = "美元" },
            new Currency { Code = "GBP", ChineseName = "英鎊" },
            new Currency { Code = "EUR", ChineseName = "歐元" }
        );
        _context.SaveChanges();
    }

    /// <summary>
    /// 測試原始 API 回傳
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetOriginal_ReturnsSuccessAndCorrectData()
    {
        // Act
        var response = await _client.GetAsync("/api/Coindesk/original");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CoindeskApiResponse>(content);
        
        Assert.NotNull(result);
        Assert.NotNull(result.Bpi);
        Assert.NotNull(result.Bpi.USD);
    }

    /// <summary>
    ///  測試客製化格式的回傳
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetCustom_ReturnsSuccessAndCorrectFormat()
    {
        // 檢查並輸出 DB 資料
        var currencies = await _context.Currencies.ToListAsync();
        _output.WriteLine($"Current currencies in DB: {JsonSerializer.Serialize(currencies)}");

        // Act
        var response = await _client.GetAsync("/api/Coindesk/custom");
        
        // 輸出回應內容以便偵錯
        var content = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response content: {content}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = JsonSerializer.Deserialize<CustomRateResponse>(content);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result.currencies);
    }
}