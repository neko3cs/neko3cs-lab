using Microsoft.EntityFrameworkCore;

namespace AzureServerlessWebApi.Data;

/// <summary>
/// データベースへの接続と操作を管理するコンテキストクラスです。
/// </summary>
/// <param name="options">データベースの接続設定など</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// User テーブルに対応するデータセットです。
    /// これを介してユーザーの追加・取得・更新・削除を行います。
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// データベースのモデル（テーブル構造など）を構築する際の詳細設定を行います。
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User クラスを「dbo.User」テーブルにマッピングします。
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", schema: "dbo");
        });
    }
}
