using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureServerlessWebApi.Data;

/// <summary>
/// データベースの「User」テーブルに対応するデータモデルです。
/// </summary>
[Table("User", Schema = "dbo")]
public class User
{
    /// <summary>
    /// ユーザーID（主キー）
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// ユーザー名
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// メールアドレス
    /// </summary>
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
}
