using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoCurrencyApi.Domain.Entities;
using CryptoCurrencyApi.Domain.Repositories;
using CryptoCurrencyApi.Infrastructure.Data;

namespace CryptoCurrencyApi.Infrastructure.Repositories
{
    /// <summary>
    /// 幣別資料倉儲實作
    /// </summary>
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// 新增幣別
        /// </summary>
        public async Task<Currency> AddAsync(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            if (await ExistsAsync(currency.Code))
                throw new InvalidOperationException($"幣別代碼 {currency.Code} 已存在");

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        /// <summary>
        /// 根據幣別代碼取得幣別
        /// </summary>
        public async Task<Currency> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("幣別代碼不能為空", nameof(code));

            return await _context.Currencies
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        /// <summary>
        /// 更新幣別資訊
        /// </summary>
        public async Task<Currency> UpdateAsync(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            var existingCurrency = await GetByCodeAsync(currency.Code);
            if (existingCurrency == null)
                throw new InvalidOperationException($"幣別代碼 {currency.Code} 不存在");

            existingCurrency.ChineseName = currency.ChineseName;
            existingCurrency.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingCurrency;
        }

        /// <summary>
        /// 刪除幣別
        /// </summary>
        public async Task DeleteAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("幣別代碼不能為空", nameof(code));

            var currency = await GetByCodeAsync(code);
            if (currency == null)
                throw new InvalidOperationException($"幣別代碼 {code} 不存在");

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 取得所有幣別，可依照代碼排序
        /// </summary>
        public async Task<IEnumerable<Currency>> ListAsync(bool orderByCode = true)
        {
            var query = _context.Currencies.AsQueryable();

            if (orderByCode)
                query = query.OrderBy(c => c.Code);

            return await query.ToListAsync();
        }

        /// <summary>
        /// 檢查幣別是否已存在
        /// </summary>
        public async Task<bool> ExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return await _context.Currencies.AnyAsync(c => c.Code == code);
        }
    }
}