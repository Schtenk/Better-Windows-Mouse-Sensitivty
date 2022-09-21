using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Better_Windows_Mouse_Sensitivty.ViewModels
{
    public class PopupViewModel: ObservableObject
    {
        public PopupViewModel()
        {
            _okCommand = new RelayCommand(() => ButtonCommand(PopupResult.OK));
            _cancelCommad = new RelayCommand(() => ButtonCommand(PopupResult.Cancel));
            _noCommand = new RelayCommand(() => ButtonCommand(PopupResult.No));
            _yesCommand = new RelayCommand(() => ButtonCommand(PopupResult.Yes));
        }

        private ICommand _okCommand;
        public ICommand OKCommand { get => _okCommand; set => SetProperty(ref _okCommand, value); }

        private ICommand _cancelCommad;
        public ICommand CancelCommand { get => _cancelCommad; set => SetProperty(ref _cancelCommad, value); }

        private ICommand _noCommand;
        public ICommand NoCommand { get => _noCommand; set => SetProperty(ref _noCommand, value); }

        private ICommand _yesCommand;
        public ICommand YesCommand { get => _yesCommand; set => SetProperty(ref _yesCommand, value); }

        private string _windowTitle = "Title";
        public string WindowTitle { get => _windowTitle; set => SetProperty(ref _windowTitle, value); }

        private string _message = "Message";
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        private PopupButtons _buttons = PopupButtons.OK;
        public PopupButtons Buttons { get => _buttons; set => SetProperty(ref _buttons, value); }

        public PopupResult PopupResult { get; set; } = PopupResult.None;
        public Action? CloseAction { get; set; }

        public void ButtonCommand(PopupResult popupResult)
        {
            PopupResult = popupResult;
            if(CloseAction != null) CloseAction();
        }
    }
    public enum PopupButtons
    {
        OK,
        OKCancel,
        YesNoCancel,
        YesNo
    }

    public enum PopupResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }
}
