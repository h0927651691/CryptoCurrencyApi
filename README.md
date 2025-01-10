# 加密貨幣API專案
這是一個使用 ASP.NET Core 8.0 開發的加密貨幣 API 專案。主要功能包含幣別資訊查詢及維護,並串接 Coindesk API 提供即時匯率資訊。
## 安裝說明

1.安裝必要軟體:

 - 安裝 Docker Desktop
 - 安裝 Docker Compose


2.執行以下指令啟動服務:
```bash
docker compose up -d
```

3.停止服務:
```bash
docker compose down
```
4.查看服務狀態:
```bash
docker compose ps
```
## 使用方式
docker服務啟動完成後透過以下連結使用
```
http://localhost:5001/swagger/index.html
```
使用以下指令進行單元測試
```
dotnet test CryptoCurrencyApi.Tests/CryptoCurrencyApi.Tests.csproj     
```
> [!IMPORTANT]  
> 測試資料在容器啟動後20秒會自動建立

## 目錄結構

- CryptoCurrencyApi.Api: API層,處理HTTP請求和回應
- CryptoCurrencyApi.Application: 應用層,處理業務邏輯
- CryptoCurrencyApi.Domain: 領域層,定義核心模型
- CryptoCurrencyApi.Infrastructure: 基礎設施層,處理資料存取
- CryptoCurrencyApi.Tests: 單元測試及整合測試
- docker: Docker 相關設定檔

    - api: API服務的Dockerfile
    - sqlserver: 資料庫初始化腳本