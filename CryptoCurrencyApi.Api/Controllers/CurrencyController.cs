using Microsoft.AspNetCore.Mvc;
using CryptoCurrencyApi.Application.Services;
using CryptoCurrencyApi.Application.DTOs;

namespace CryptoCurrencyApi.Api.Controllers
{
    /// <summary>
    /// 幣別管理控制器
    /// </summary>
    
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="currencyService">幣別服務</param>
        /// <param name="logger">日誌服務</param>
        public CurrencyController(
            CurrencyService currencyService,
            ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/Currency - 取得所有幣別資料
        /// <summary>
        /// 取得所有幣別資料
        /// </summary>
        /// <returns>所有幣別資料的清單</returns>
        /// <response code="200">成功取得幣別清單</response>
        /// <response code="500">處理請求時發生錯誤</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAll()
        {
            try
            {
                 // 呼叫服務層取得所有幣別資料
                var currencies = await _currencyService.ListCurrenciesAsync();
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得所有幣別時發生錯誤");
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }

        // GET api/Currency/{code} - 依據代碼取得特定幣別
        /// <summary>
        /// 依據代碼取得特定幣別
        /// </summary>
        /// <param name="code">幣別代碼</param>
        /// <returns>指定代碼的幣別資料</returns>
        /// <response code="200">成功取得幣別資料</response>
        /// <response code="404">找不到指定的幣別代碼</response>
        /// <response code="500">處理請求時發生錯誤</response>
        [HttpGet("{code}")]
        public async Task<ActionResult<CurrencyDto>> GetByCode(string code)
        {
            try
            {
                var currency = await _currencyService.GetCurrencyByCodeAsync(code);
                return Ok(currency);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"找不到代碼為 {code} 的幣別");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得幣別 {Code} 時發生錯誤", code);
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }
        /// <summary>
        /// 新增幣別
        /// </summary>
        /// <param name="createDto">新增幣別的資料</param>
        /// <returns>新建立的幣別資料</returns>
        /// <response code="201">成功建立幣別</response>
        /// <response code="400">輸入的資料無效</response>
        /// <response code="500">處理請求時發生錯誤</response>
         // POST api/Currency - 新增幣別
        [HttpPost]
        public async Task<ActionResult<CurrencyDto>> Create([FromBody] CreateCurrencyDto createDto)
        {
            try
            {
                var currency = await _currencyService.AddCurrencyAsync(createDto);
                 // 回傳201 Created狀態碼與新建立的資源位置
                return CreatedAtAction(nameof(GetByCode), new { code = currency.Code }, currency);
            }
            catch (ArgumentException ex)
            {
                // 輸入資料驗證失敗時回傳400
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增幣別時發生錯誤");
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }

        // PUT api/Currency/{code} - 更新幣別資訊
        /// <summary>
        /// 更新幣別資訊
        /// </summary>
        /// <param name="code">幣別代碼</param>
        /// <param name="updateDto">更新幣別的資料</param>
        /// <returns>無內容</returns>
        /// <response code="204">成功更新幣別</response>
        /// <response code="400">輸入的資料無效</response>
        /// <response code="404">找不到指定的幣別代碼</response>
        /// <response code="500">處理請求時發生錯誤</response>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string code, [FromBody] UpdateCurrencyDto updateDto)
        {
            try
            {
                await _currencyService.UpdateCurrencyAsync(code, updateDto.ChineseName);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"找不到代碼為 {code} 的幣別");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新幣別 {Code} 時發生錯誤", code);
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }
        /// <summary>
        /// 刪除幣別
        /// </summary>
        /// <param name="code">要刪除的幣別代碼</param>
        /// <returns>無內容</returns>
        /// <response code="204">成功刪除幣別</response>
        /// <response code="404">找不到指定的幣別代碼</response>
        /// <response code="500">處理請求時發生錯誤</response>
        // DELETE api/Currency/{code} - 刪除幣別
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                await _currencyService.DeleteCurrencyAsync(code);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"找不到代碼為 {code} 的幣別");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除幣別 {Code} 時發生錯誤", code);
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }
    }
    /// <summary>
    /// Coindesk API 控制器
    /// 處理與 Coindesk API 相關的請求
    /// </summary>
    // 處理 Coindesk API 相關請求的控制器
    [ApiController]
    [Route("api/[controller]")]
    public class CoindeskController : ControllerBase
    {
        private readonly ICoindeskService _coindeskService;
        private readonly ILogger<CoindeskController> _logger;

        public CoindeskController(
            ICoindeskService coindeskService,
            ILogger<CoindeskController> logger)
        {
            _coindeskService = coindeskService ?? throw new ArgumentNullException(nameof(coindeskService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// 取得原始 Coindesk API 資料
        /// </summary>
        /// <returns>原始的 Coindesk API 回應資料</returns>
        /// <response code="200">成功取得 Coindesk API 資料</response>
        /// <response code="500">處理請求時發生錯誤</response>
        // GET api/Coindesk/original - 取得原始 Coindesk API 資料
        [HttpGet("original")]
        public async Task<ActionResult<CoindeskApiResponse>> GetOriginalPrice()
        {
            try
            {
                var result = await _coindeskService.GetCurrentPriceAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得原始 Coindesk 資料時發生錯誤");
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }
        /// <summary>
        /// 取得經過轉換的匯率資料
        /// 將 Coindesk API 的資料轉換為自訂格式，並加入中文幣別名稱
        /// </summary>
        /// <returns>自訂格式的匯率資料，包含中文幣別名稱</returns>
        /// <response code="200">成功取得轉換後的匯率資料</response>
        /// <response code="500">處理請求時發生錯誤</response>
        // GET api/Coindesk/custom - 取得經過轉換的匯率資料
        [HttpGet("custom")]
        [ProducesResponseType(typeof(CustomRateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomRateResponse>> GetCustomRate()
        {
            try
            {
                var result = await _coindeskService.GetCustomRateAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得自訂匯率資料時發生錯誤");
                return StatusCode(500, "處理請求時發生錯誤");
            }
        }
    }
}