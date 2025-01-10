using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CryptoCurrencyApi.Infrastructure.Data;

namespace CryptoCurrencyApi.Tests.Helpers;

public abstract class TestBase
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly ApplicationDbContext Context;

    protected TestBase()
    {
        Factory = new CustomWebApplicationFactory();
        Client = Factory.CreateClient();
        var scope = Factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}