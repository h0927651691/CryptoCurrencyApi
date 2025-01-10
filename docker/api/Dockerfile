# 建構階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 設定記憶體限制
ENV DOTNET_GCHeapHardLimit=200000000

# 只複製必要的檔案
COPY ["CryptoCurrencyApi.Api/CryptoCurrencyApi.Api.csproj", "CryptoCurrencyApi.Api/"]
COPY ["CryptoCurrencyApi.Application/CryptoCurrencyApi.Application.csproj", "CryptoCurrencyApi.Application/"]
COPY ["CryptoCurrencyApi.Domain/CryptoCurrencyApi.Domain.csproj", "CryptoCurrencyApi.Domain/"]
COPY ["CryptoCurrencyApi.Infrastructure/CryptoCurrencyApi.Infrastructure.csproj", "CryptoCurrencyApi.Infrastructure/"]

# 還原相依套件
RUN dotnet restore "CryptoCurrencyApi.Api/CryptoCurrencyApi.Api.csproj"

# 複製其餘源碼
COPY . .

# 發布
RUN dotnet publish "CryptoCurrencyApi.Api/CryptoCurrencyApi.Api.csproj" -c Release -o /app/publish --no-restore

# 執行階段映像檔
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CryptoCurrencyApi.Api.dll"]