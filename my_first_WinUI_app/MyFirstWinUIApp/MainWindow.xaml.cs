using Microsoft.UI.Xaml;
using MyFirstWinUIApp.ViewModels;

namespace MyFirstWinUIApp
{
    /// <summary>
    /// アプリのメインウィンドウ（View）。
    /// ロジックは持たず、ViewModel をプロパティとして公開して XAML 側から x:Bind する。
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>XAML から x:Bind するための ViewModel。</summary>
        public MainViewModel ViewModel { get; } = new();

        public MainWindow()
        {
            InitializeComponent();

            // 既定だとウィンドウが小さいため、サンプルが見やすいサイズへ調整する。
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32(1100, 720));
        }
    }
}
