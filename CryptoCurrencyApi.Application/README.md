# CryptoCurrencyApi.Application

應用層包含業務邏輯實作和 DTO 定義。

## 目錄結構
- `DTOs/`: 資料傳輸物件
  - `CoindeskDtos.cs`: Coindesk API 相關的 DTO
  - `CurrencyDtos.cs`: 幣別資料相關的 DTO
- `Services/`: 服務實作
  - `CoindeskService.cs`: 處理 Coindesk API 整合
  - `CurrencyService.cs`: 處理幣別資料邏輯

## 主要功能
- 實作業務邏輯
- 資料轉換和驗證
- 外部 API 整合
