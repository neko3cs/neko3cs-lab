using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MyFirstWinUIApp.Models
{
    /// <summary>
    /// 社員ひとり分のデータを表すエンティティ（Model）。
    /// <para>
    /// ObservableObject を継承しているため、各プロパティを変更すると
    /// PropertyChanged 通知が飛び、バインドされた画面コントロールへ自動的に反映される。
    /// [ObservableProperty] を付けたフィールドからは、同名の PascalCase プロパティ
    /// （例: _name → Name）がソースジェネレーターによって生成される。
    /// </para>
    /// </summary>
    public partial class Person : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        private string _department = string.Empty;

        // IsActive が変わったら、それを元に計算している StatusText も更新通知を出す。
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(StatusText))]
        private bool _isActive = true;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(JoinedDateText))]
        private DateTimeOffset _joinedDate = DateTimeOffset.Now;

        /// <summary>在籍状況を表す表示用テキスト（コンバーターを使わずに済ませるための計算プロパティ）。</summary>
        public string StatusText => IsActive ? "在籍中" : "退職";

        /// <summary>入社日を画面表示用に整形した文字列。</summary>
        public string JoinedDateText => JoinedDate.ToString("yyyy/MM/dd");
    }
}
