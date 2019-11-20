using PropertyChanged;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace XamNumericEditorFocusSample.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ModalWindowViewModel
    {
        public ReactiveProperty<int?> NumberInput { get; } = new ReactiveProperty<int?>();
        public ReactiveProperty<int?> NumberOutput { get; } = new ReactiveProperty<int?>();
        public ReactiveCommand ShowTextCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ClearTextCommand { get; }
        public ReactiveCommand FocusTextBoxCommand { get; } = new ReactiveCommand();

        public ModalWindowViewModel()
        {
            ShowTextCommand.Subscribe(_ =>
            {
                NumberOutput.Value = NumberInput.Value;
                NumberInput.Value = null;
            });

            ClearTextCommand = NumberOutput
                .Select(number => !(number is null))
                .ToReactiveCommand();
            ClearTextCommand.Subscribe(_ =>
            {
                NumberOutput.Value = null;
                NumberInput.Value = null;
            });
        }
    }
}
