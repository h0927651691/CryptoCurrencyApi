using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoCurrencyApi.Application.DTOs
{
    /// <summary>
    /// 用於創建新幣別的 DTO
    /// </summary>
    public class CreateCurrencyDto
    {
        /// <summary>
        /// 幣別代碼
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "幣別代碼必須是3個大寫英文字母")]
        public string Code { get; set; }

        /// <summary>
        /// 幣別中文名稱
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "幣別中文名稱長度必須介於2-50個字元")]
        public string ChineseName { get; set; }
    }

    /// <summary>
    /// 用於更新幣別資訊的 DTO
    /// </summary>
    public class UpdateCurrencyDto
    {
        /// <summary>
        /// 幣別中文名稱
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "幣別中文名稱長度必須介於2-50個字元")]
        public string ChineseName { get; set; }
    }

    /// <summary>
    /// 用於查詢回傳的幣別 DTO
    /// </summary>
    public class CurrencyDto
    {
        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 幣別中文名稱
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}