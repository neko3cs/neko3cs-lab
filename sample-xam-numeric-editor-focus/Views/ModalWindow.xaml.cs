using System.Windows.Controls;
using XamNumericEditorFocusSample.ViewModels;

namespace XamNumericEditorFocusSample.Views
{
    /// <summary>
    /// ModalWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ModalWindow : UserControl
    {
        public ModalWindow()
        {
            InitializeComponent();

            var viewModel = DataContext as ModalWindowViewModel;
            viewModel.FocusTextBoxCommand.Subscribe(
                () => txtNumber.Focus()
            );
        }
    }
}
