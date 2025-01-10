using Microsoft.EntityFrameworkCore;
using CryptoCurrencyApi.Domain.Entities;

namespace CryptoCurrencyApi.Infrastructure.Data
{
    /// <summary>
    /// 應用程式資料庫上下文
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 幣別資料集
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// 配置模型建立時的行為
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置 Currency 實體
            modelBuilder.Entity<Currency>(entity =>
            {
                // 設定主鍵
                entity.HasKey(c => c.Code);

                // 設定 Code 為不可為空且長度為 3
                entity.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(3);

                // 設定 ChineseName 為不可為空且最大長度為 50
                entity.Property(c => c.ChineseName)
                    .IsRequired()
                    .HasMaxLength(50);

                // 配置建立時間
                entity.Property(c => c.CreatedAt)
                    .IsRequired();

                // 配置更新時間（可為空）
                entity.Property(c => c.UpdatedAt)
                    .IsRequired(false);

                // 建立唯一索引
                entity.HasIndex(c => c.Code)
                    .IsUnique();
            });
        }
    }
}