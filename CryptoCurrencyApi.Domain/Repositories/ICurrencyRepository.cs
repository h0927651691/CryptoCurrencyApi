using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoCurrencyApi.Domain.Entities;

namespace CryptoCurrencyApi.Domain.Repositories
{
    /// <summary>
    /// 幣別資料倉儲介面
    /// </summary>
    public interface ICurrencyRepository
    {
        /// <summary>
        /// 新增幣別
        /// </summary>
        Task<Currency> AddAsync(Currency currency);

        /// <summary>
        /// 根據幣別代碼取得幣別
        /// </summary>
        Task<Currency> GetByCodeAsync(string code);

        /// <summary>
        /// 更新幣別資訊
        /// </summary>
        Task<Currency> UpdateAsync(Currency currency);

        /// <summary>
        /// 刪除幣別
        /// </summary>
        Task DeleteAsync(string code);

        /// <summary>
        /// 取得所有幣別，可依照代碼排序
        /// </summary>
        Task<IEnumerable<Currency>> ListAsync(bool orderByCode = true);

        /// <summary>
        /// 檢查幣別是否已存在
        /// </summary>
        Task<bool> ExistsAsync(string code);
    }
}