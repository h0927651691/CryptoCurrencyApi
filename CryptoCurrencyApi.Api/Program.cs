using Microsoft.EntityFrameworkCore;
using CryptoCurrencyApi.Infrastructure.Data;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Infrastructure.Repositories;
using CryptoCurrencyApi.Application.Services;
using CryptoCurrencyApi.Api.Middleware;
using System.Reflection;
public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });



        builder.Services.AddLogging(config => 
        {
            config.SetMinimumLevel(LogLevel.Debug)
                    .AddConsole()  // 將日誌輸出到控制台
                    .AddDebug()// 將日誌輸出到 Debug 視窗
                    .AddFile("Logs/test-{Date}.txt");   
        });
        // Configure services based on environment
        if (builder.Environment.IsEnvironment("Testing"))
        {
            // Let the test configuration handle services

        }
        else 
        {
            // Configure DbContext for non-test environment
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
            // Register HttpClient and other services
            builder.Services.AddHttpClient("CoinDeskApi");
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<CurrencyService>();
            builder.Services.AddScoped<ICoindeskService, CoindeskService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}