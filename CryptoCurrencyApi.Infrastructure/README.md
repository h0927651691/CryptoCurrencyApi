# CryptoCurrencyApi.Infrastructure

基礎設施層負責實作資料存取和外部服務整合。

## 目錄結構
- `Data/`: 資料存取相關
  - `ApplicationDbContext.cs`: Entity Framework Core 資料庫上下文
- `Repositories/`: 儲存庫實作
  - `CurrencyRepository.cs`: 幣別儲存庫實作

## 主要功能
- 實作資料庫存取
- 處理資料持久化
- 整合外部服務