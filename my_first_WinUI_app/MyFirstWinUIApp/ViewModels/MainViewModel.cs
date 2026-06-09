using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFirstWinUIApp.Models;

namespace MyFirstWinUIApp.ViewModels
{
    /// <summary>
    /// メイン画面のロジックと状態を担う ViewModel。
    /// <para>
    /// View（MainWindow.xaml）はこの ViewModel のプロパティ／コマンドにのみバインドし、
    /// コントロールを直接触らない。これが MVVM の基本的な役割分担。
    /// </para>
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        /// <summary>一覧表示用のコレクション。ObservableCollection なので追加・削除が即座に ListView へ反映される。</summary>
        public ObservableCollection<Person> People { get; } = new();

        /// <summary>ComboBox の選択肢（部署）。固定リストなので読み取り専用で公開する。</summary>
        public IReadOnlyList<string> Departments { get; } =
            new[] { "営業部", "開発部", "総務部", "人事部" };

        // ---- 入力フォームとバインドするプロパティ群 ----

        // NewName が変わるたびに「追加」ボタンの有効/無効（CanExecute）を再評価させる。
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPersonCommand))]
        private string _newName = string.Empty;

        // NumberBox.Value は double 型なので、入力用は double で受ける。
        [ObservableProperty]
        private double _newAge = 25;

        [ObservableProperty]
        private string? _newDepartment = "開発部";

        [ObservableProperty]
        private bool _newIsActive = true;

        // CalendarDatePicker.Date は DateTimeOffset? なので nullable で受ける。
        [ObservableProperty]
        private DateTimeOffset? _newJoinedDate = DateTimeOffset.Now;

        // 一覧で選択された行。変わるたびに「削除」ボタンの有効/無効を再評価させる。
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemovePersonCommand))]
        private Person? _selectedPerson;

        /// <summary>現在の登録人数。People の増減に追従して通知する。</summary>
        public int PeopleCount => People.Count;

        public MainViewModel()
        {
            // コレクションの増減に合わせて人数表示を更新する。
            People.CollectionChanged += (_, _) => OnPropertyChanged(nameof(PeopleCount));

            // 起動時に画面が空にならないよう、サンプルデータを投入しておく。
            People.Add(new Person { Name = "山田 太郎", Age = 32, Department = "開発部", IsActive = true, JoinedDate = new DateTimeOffset(2018, 4, 1, 0, 0, 0, TimeSpan.FromHours(9)) });
            People.Add(new Person { Name = "佐藤 花子", Age = 27, Department = "営業部", IsActive = true, JoinedDate = new DateTimeOffset(2021, 10, 1, 0, 0, 0, TimeSpan.FromHours(9)) });
            People.Add(new Person { Name = "鈴木 一郎", Age = 45, Department = "総務部", IsActive = false, JoinedDate = new DateTimeOffset(2005, 7, 16, 0, 0, 0, TimeSpan.FromHours(9)) });
        }

        /// <summary>入力内容を元に新しい社員を一覧へ追加する。</summary>
        [RelayCommand(CanExecute = nameof(CanAddPerson))]
        private void AddPerson()
        {
            People.Add(new Person
            {
                Name = NewName.Trim(),
                Age = (int)NewAge,
                Department = NewDepartment ?? "未設定",
                IsActive = NewIsActive,
                JoinedDate = NewJoinedDate ?? DateTimeOffset.Now,
            });

            // 連続入力しやすいよう、入力欄を初期状態へ戻す。
            NewName = string.Empty;
            NewAge = 25;
            NewDepartment = "開発部";
            NewIsActive = true;
            NewJoinedDate = DateTimeOffset.Now;
        }

        // 氏名が空のときは追加させない（ボタンが自動的に無効化される）。
        private bool CanAddPerson() => !string.IsNullOrWhiteSpace(NewName);

        /// <summary>一覧で選択中の社員を削除する。</summary>
        [RelayCommand(CanExecute = nameof(CanRemovePerson))]
        private void RemovePerson()
        {
            if (SelectedPerson is not null)
            {
                People.Remove(SelectedPerson);
            }
        }

        // 何も選択されていなければ削除させない。
        private bool CanRemovePerson() => SelectedPerson is not null;
    }
}
