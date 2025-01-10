using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CryptoCurrencyApi.Domain.Entities
{
    /// <summary>
    /// 代表幣別的領域實體
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// 幣別代碼（唯一識別碼）
        /// </summary>
        [Key]
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

        /// <summary>
        /// 幣別建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 幣別最後更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 預設建構函式
        /// </summary>
        public Currency()
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 帶參數的建構函式
        /// </summary>
        /// <param name="code">幣別代碼</param>
        /// <param name="chineseName">幣別中文名稱</param>
        public Currency(string code, string chineseName) : this()
        {
            Code = ValidateCurrencyCode(code);
            ChineseName = ValidateChineseName(chineseName);
        }

        /// <summary>
        /// 驗證幣別代碼
        /// </summary>
        private static string ValidateCurrencyCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("幣別代碼不能為空", nameof(code));

            if (!Regex.IsMatch(code, @"^[A-Z]{3}$"))
                throw new ArgumentException("幣別代碼必須是3個大寫英文字母", nameof(code));

            return code;
        }

        /// <summary>
        /// 驗證中文名稱
        /// </summary>
        private static string ValidateChineseName(string chineseName)
        {
            if (string.IsNullOrWhiteSpace(chineseName))
                throw new ArgumentException("幣別中文名稱不能為空", nameof(chineseName));

            return chineseName;
        }

        /// <summary>
        /// 更新幣別資訊
        /// </summary>
        public void Update(string chineseName)
        {
            ChineseName = ValidateChineseName(chineseName);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}