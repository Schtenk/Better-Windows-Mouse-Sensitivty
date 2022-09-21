using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Better_Windows_Mouse_Sensitivty.ViewModels;

namespace Better_Windows_Mouse_Sensitivty.Views
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public PopupWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <param name="window">Application.Current.MainWindow if null</param>
        /// <returns></returns>
        public static PopupResult ShowDialog(string title, string message, PopupButtons buttons = PopupButtons.OK, Window? window = null)
        {
            window = window ?? Application.Current.MainWindow;

            var popupWindow = new PopupWindow();
            var popupVM = new PopupViewModel();
            popupVM.WindowTitle = title;
            popupVM.Message = message;
            popupVM.Buttons = buttons;
            popupVM.CloseAction = popupWindow.Close;

            popupWindow.Owner = window;
            popupWindow.DataContext = popupVM;
            popupWindow.ShowDialog();

            return popupVM.PopupResult;
        }
    }

}
