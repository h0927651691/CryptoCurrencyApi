using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CryptoCurrencyApi.Infrastructure.Data;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Infrastructure.Repositories;
using CryptoCurrencyApi.Application.Services;
using CryptoCurrencyApi.Domain.Entities;

namespace CryptoCurrencyApi.Tests.Helpers;

/// <summary>
/// 自訂的 Web 應用程式工廠，用於整合測試
/// 提供測試環境所需的資料庫、HTTP 客戶端和測試資料
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // 儲存 Mock HTTP 處理器的實例
    private readonly MockHttpMessageHandler _mockHandler;

    /// <summary>
    /// 建構子: 初始化測試用的 Web 應用程式工廠
    /// </summary>
    public CustomWebApplicationFactory()
    {
        _mockHandler = new MockHttpMessageHandler();
    }

    /// <summary>
    /// 設定模擬 HTTP 客戶端是否要回傳失敗的回應
    /// </summary>
    /// <param name="shouldFail">是否模擬失敗的回應</param>
    public void SetMockHttpClientShouldFail(bool shouldFail)
    {
        _mockHandler.SetShouldFail(shouldFail);
    }

    /// <summary>
    /// 設定測試環境的 Web 主機
    /// 包含設定記憶體資料庫、HTTP 客戶端和初始化測試資料
    /// </summary>
    /// <param name="builder">Web 主機建構器</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            ConfigureHttpClient(services);

            InitializeTestData(services);
        });
    }

    /// <summary>
    /// 設定測試用的 HTTP 客戶端
    /// 移除原有的 HTTP 客戶端工廠並註冊新的模擬客戶端
    /// </summary>
    /// <param name="services">服務集合</param>
    private void ConfigureHttpClient(IServiceCollection services)
    {
        var httpClientDescriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IHttpClientFactory));

        if (httpClientDescriptor != null)
        {
            services.Remove(httpClientDescriptor);
        }

        // 註冊新的 HTTP 客戶端，使用模擬的處理器
        services.AddHttpClient("CoinDeskApi")
            .ConfigurePrimaryHttpMessageHandler(() => _mockHandler);
    }

    /// <summary>
    /// 初始化測試資料
    /// 建立資料庫並新增基本的貨幣資料
    /// </summary>
    /// <param name="services">服務集合</param>
    private void InitializeTestData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        db.Database.EnsureCreated();

        if (!db.Currencies.Any())
        {
            db.Currencies.AddRange(
                new Currency { Code = "USD", ChineseName = "美元" },
                new Currency { Code = "GBP", ChineseName = "英鎊" },
                new Currency { Code = "EUR", ChineseName = "歐元" }
            );
            db.SaveChanges();
        }
    }
}