# CryptoCurrencyApi.Api

API 專案是應用程式的入口點,負責處理 HTTP 請求和回應。

## 目錄結構
- `Controllers/`: API 端點控制器
  - `CurrencyController.cs`: 處理幣別相關的 CRUD 操作
- `Middleware/`: 自定義中間件
  - `ExceptionHandlingMiddleware.cs`: 全域例外處理
- `Properties/`: 專案設定
  - `launchSettings.json`: 啟動設定

## 主要功能
- 提供 RESTful API 端點
- 全域錯誤處理
- Swagger API 文件