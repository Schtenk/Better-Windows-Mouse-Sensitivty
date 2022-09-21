using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Better_Windows_Mouse_Sensitivty.ViewModels;
using Better_Windows_Mouse_Sensitivty.Views;
using System.Windows;

namespace Better_Windows_Mouse_Sensitivty.Helper
{
    public class PopupDialog
    {
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
