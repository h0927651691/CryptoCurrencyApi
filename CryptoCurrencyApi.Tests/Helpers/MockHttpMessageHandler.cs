using System.Net;
using System.Text;
using System.Text.Json;

/// <summary>
/// 用於測試環境的 HTTP 訊息處理器
/// 可模擬 HTTP 請求的回應，支援成功與失敗的情境
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    // 控制是否回傳失敗的回應
    private bool _shouldFail;
    // 儲存預設的模擬回應內容
    private readonly string _mockResponse;

    /// <summary>
    /// 建構子: 初始化 Mock HTTP 處理器
    /// </summary>
    /// <param name="mockResponse">自訂的模擬回應內容。若未提供則使用預設回應</param>
    public MockHttpMessageHandler(string mockResponse = null)
    {
        _shouldFail = false;
        _mockResponse = mockResponse ?? GetMockResponse();
    }
    /// <summary>
    /// 設定是否要模擬失敗的回應
    /// true: 回傳 500 Internal Server Error
    /// false: 回傳正常的模擬資料
    /// </summary>
    /// <param name="shouldFail">是否要回傳失敗的回應</param>
    public void SetShouldFail(bool shouldFail)
    {
        _shouldFail = shouldFail;
    }
    /// <summary>
    /// 處理 HTTP 請求並回傳模擬回應
    /// 根據 shouldFail 旗標決定回傳成功或失敗的回應
    /// </summary>
    /// <param name="request">HTTP 請求訊息</param>
    /// <param name="cancellationToken">取消作業的 Token</param>
    /// <returns>模擬的 HTTP 回應訊息</returns>
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_shouldFail)
        {
            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        return Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(GetMockResponse())
            });
    }
    /// <summary>
    /// 取得預設的模擬回應內容
    /// 回傳模擬的加密貨幣價格資料，包含 USD、GBP 和 EUR 的匯率
    /// </summary>
    /// <returns>JSON 格式的模擬回應字串</returns>
    private string GetMockResponse()
    {
        return @"{
                ""time"": {
                    ""updated"": ""Oct 15, 2023 00:00:00 UTC"",
                    ""updatedISO"": ""2023-10-15T00:00:00+00:00""
                },
                ""bpi"": {
                    ""USD"": {
                        ""code"": ""USD"",
                        ""rate"": ""30,000.0000"",
                        ""description"": ""United States Dollar"",
                        ""rate_float"": 30000.0
                    },
                    ""GBP"": {
                        ""code"": ""GBP"", 
                        ""rate"": ""24,000.0000"",
                        ""description"": ""British Pound Sterling"",
                        ""rate_float"": 24000.0
                    },
                    ""EUR"": {
                        ""code"": ""EUR"",
                        ""rate"": ""28,000.0000"", 
                        ""description"": ""Euro"",
                        ""rate_float"": 28000.0
                    }
                }
            }";
    }
}